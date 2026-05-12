using Assets.Project.CodeBase.UI.LoadingScreen;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Project.Scripts.UI.LoadingScreen
{
    public class SceneTransition : MonoBehaviour
    {
        /// <summary>
        /// Скрипт переключения между сценами, отвечает за канвас который затемняется 
        /// </summary>
        public static SceneTransition Instance;
        public CanvasGroup _canvasGroup;
        public TMP_Text _version_text;
        public LoadingUI _loadingProgress;
        public float _blackoutAnimTime = 1;
        public float _minTimeLoading = 0;
        public UnityEvent<float> OnProgress = new();

        private CancellationTokenSource _cts;
        private float _progress;

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
            {
                _loadingProgress.Initialize(this);
            }
        }

        public async UniTask StartAnimation()
        {
            if (_canvasGroup)
            {
                _canvasGroup.alpha = 0; 

                if (_loadingProgress != null)
                {
                    _loadingProgress.gameObject.SetActive(true);
                }

                if (_version_text)
                    _version_text.text = $"Version: {Application.version}";

                _canvasGroup.gameObject.SetActive(true);
                SetProgress(0);

                await DOVirtual.Float(0, 1, _blackoutAnimTime, (t) => _canvasGroup.alpha = t)
                    .SetUpdate(true)
                    .AsyncWaitForCompletion();
            }
        }
        public void SetProgress(float value)
        {
            _progress = Mathf.Clamp01(value);
            OnProgress?.Invoke(_progress);
        }
        public async UniTask EndAnimation()
        {
            if (_canvasGroup)
            {
                await DOVirtual.Float(1, 0, _blackoutAnimTime, (t) => _canvasGroup.alpha = t)
                    .SetUpdate(true)
                    .AsyncWaitForCompletion();

                _canvasGroup.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

    }
}
