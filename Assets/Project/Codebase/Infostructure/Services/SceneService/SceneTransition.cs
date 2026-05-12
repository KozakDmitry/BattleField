using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Collections;
using Assets.Project.CodeBase.Extentions;



namespace Old
{

    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance;
        public static Queue<string> QueueLoading = new();

        public static int _batchTotal = 1;
        public static int _batchCurrent = 0;

        static bool _isFromGdpr = true;

        public TMP_Text Version_text;
        public CanvasGroup canvasGroup;
        public float BlackoutAnimTime = 1;
        public float MinTimeLoading = 0;
        public UnityEvent<float> OnProgress = new();
        public static Action<Scene> OnStartLoadScene;
        public static Action<string> OnSceneLoading;
        public static Action OnEndLoadScene;
        public static bool IsLoading = false;
        public bool isIgnoreStartAnim = false;

        Scene _currentScene;
        float progress;
        float DeltaTime = 0.01f;
        float StartTime;

        private CancellationTokenSource _cts;

        public Scene GetCurrentScene => _currentScene;
        public Scene GetActiveScene => SceneManager.GetActiveScene();



        private void Awake()
        {
            if (canvasGroup) canvasGroup.alpha = 0;

            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameObject.SetActive(false);

            _cts = new CancellationTokenSource();
            _currentScene = SceneManager.GetActiveScene();
            DI.Register(this, RegisterMode.global);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void Load(string sceneName, bool isFromGdpr = false)
        {

            _isFromGdpr = isFromGdpr;

            if (IsLoading)
            {
                QueueLoading.Enqueue(sceneName);
                return;
            }

            DI.RemoveAllForThisScene();
            DOTween.KillAll();
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            IsLoading = true;
            QueueLoading.Enqueue(sceneName);

            _ = ProcessQueueAsync();
        }

        async UniTask ProcessQueueAsync()
        {

            await StartAnim();

            while (QueueLoading.Count > 0 && !_cts.Token.IsCancellationRequested)
            {
                if (QueueLoading.TryDequeue(out string load))
                {
                    await LoadSceneAsync((load));
                    System.GC.Collect();
                }
            }

            await UniTask.WaitUntil(() => SceneManager.sceneCount == 1);

            SceneInitializing Init = FindAnyObjectByType<SceneInitializing>();
            await UniTask.WaitUntil(() =>
            {
                Init = FindAnyObjectByType<SceneInitializing>();
                return Init != null;
            });


            await Init.Init();

            TimeManager.SetPause(false, GetType());

            //Application.runInBackground = false;

            await EndAnim();

            IsLoading = false;
            OnEndLoadScene?.Invoke();
        }

        public static async UniTask WaitAllLoad()
        {
            if (IsLoading)
            {
                await UniTask.WaitUntil(() => !IsLoading);
            }
        }

        async UniTask StartAnim()
        {
            //DI.ResolveSync<DataManager>().IsIncreaseTooltipIndex = true;

            TimeManager.ResetBlocks();

            if (canvasGroup)
            {
                if (Version_text)
                    Version_text.text = $"Version: {Application.version}";

                canvasGroup.gameObject.SetActive(true);
                SetProgress(0);

                await DOVirtual.Float(0, 1, BlackoutAnimTime, (t) => canvasGroup.alpha = t)
                    .SetUpdate(true)
                    .AsyncWaitForCompletion();

            }
        }

        async UniTask EndAnim()
        {
            if (canvasGroup)
            {
                await DOVirtual.Float(1, 0, BlackoutAnimTime, (t) => canvasGroup.alpha = t)
                    .SetUpdate(true)
                    .AsyncWaitForCompletion();

                canvasGroup.gameObject.SetActive(false);
                _isFromGdpr = false;
            }
        }

        async UniTask LoadSceneAsync(string sceneName)
        {
            //Application.runInBackground = true;
            //
            await UniTask.Yield();

            StartTime = Time.realtimeSinceStartup;
            OnStartLoadScene?.Invoke(_currentScene);

            var sceneOp = SceneManager.LoadSceneAsync(sceneName);
            sceneOp.allowSceneActivation = false;

            if (sceneOp == null)
            {
                Debug.LogWarning("НЕТ СЦЕНЫ В БИЛДЕ: " + sceneName);
                sceneOp = SceneManager.LoadSceneAsync(SceneNames.GetSceneName(SceneNames.Name.GamePlay), LoadSceneMode.Additive);
                sceneOp.allowSceneActivation = false;
            }

            OnSceneLoading?.Invoke(sceneName);

            // Фаза загрузки 0-0.9
            while (sceneOp.progress < 0.9f && !_cts.Token.IsCancellationRequested)
            {
                float totalProgress = (_batchCurrent + sceneOp.progress) / _batchTotal;
                SetProgress(totalProgress);
                await UniTask.Delay(TimeSpan.FromSeconds(DeltaTime), ignoreTimeScale: true);
            }

            // Фаза минимального времени показа
            while (RemaindTime() > 0 && !_cts.Token.IsCancellationRequested)
            {
                float totalProgress = (_batchCurrent + sceneOp.progress) / _batchTotal;
                SetProgress(totalProgress);
                await UniTask.Delay(TimeSpan.FromSeconds(DeltaTime), ignoreTimeScale: true);
            }
            sceneOp.allowSceneActivation = true;
            _batchCurrent++;

            return;
        }

        void SetProgress(float value)
        {
            progress = Mathf.Clamp01(value);
            OnProgress?.Invoke(progress);
        }

        float RemaindTime() => Mathf.Max(0, MinTimeLoading - (Time.realtimeSinceStartup - StartTime));
    }
}
