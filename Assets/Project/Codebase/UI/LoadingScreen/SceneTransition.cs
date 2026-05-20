using Assets.Project.CodeBase.UI.LoadingScreen;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Project.Scripts.UI.LoadingScreen
{
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance;

        public CanvasGroup _canvasGroup;
        public LoadingUI _loadingProgress;
        public float _blackoutAnimTime = 1;
        public UnityEvent<float> OnProgress = new();

        private CancellationTokenSource _cts;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
            _cts = new CancellationTokenSource();
            DI.Register(this, RegisterMode.global);

            if (_loadingProgress != null)
                _loadingProgress.Initialize(this);
        }

        public async UniTask StartAnimation()
        {
            if (!_canvasGroup)
                return;

            _canvasGroup.alpha = 0;

            if (_loadingProgress != null)
            {
                _loadingProgress.gameObject.SetActive(true);
                _loadingProgress.SetVersion($"Version: {Application.version}");
            }


            _canvasGroup.gameObject.SetActive(true);
            SetProgress(0);

            await DOVirtual
                .Float(0, 1, _blackoutAnimTime, t => _canvasGroup.alpha = t)
                .SetUpdate(true)
                .AsyncWaitForCompletion()
                .AsUniTask()
                .AttachExternalCancellation(_cts.Token);
        }

        public void SetProgress(float value)
        {
            OnProgress?.Invoke(Mathf.Clamp01(value));
        }

        public async UniTask EndAnimation()
        {
            if (!_canvasGroup)
                return;

            await DOVirtual
                .Float(1, 0, _blackoutAnimTime, t => _canvasGroup.alpha = t)
                .SetUpdate(true)
                .AsyncWaitForCompletion()
                .AsUniTask()
                .AttachExternalCancellation(_cts.Token);

            _canvasGroup.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
