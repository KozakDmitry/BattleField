
using Assets.Project.CodeBase.Infostructure.Input;
using Assets.Project.CodeBase.Infostructure.States;
using Assets.Project.CodeBase.UI.LoadingScreen;
using Assets.Project.Scripts.UI.LoadingScreen;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Project.CodeBase.Infostructure.Services.SceneService
{
    /// <summary>
    /// Если SceneTransition отвечает за канвас, этот сервис уже напрямую переключает сцены.
    /// </summary>
    public class SceneService : ISceneService
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;

        public delegate void OnLoad();
        public event OnLoad OnSceneLoaded;

        private bool _isLoading;
        private SceneTransition _sceneTransition;

        public SceneService(IGameStateMachine stateMachine, IInputService inputService)
        {
            _sceneLoader = new();
            _isLoading = false;
            _stateMachine = stateMachine;
            _inputService = inputService;
            DI.Resolve<SceneTransition>((scene) =>
            {
                _sceneTransition = scene;
            });
        }

        public async UniTask LoadFirstScene(string scene)
        {
            _isLoading = true;
            DI.RemoveAllForThisScene();
            await _sceneLoader.Load(scene);
            _isLoading = false;
        }

        public async UniTask LoadScene(string nextScene, Action<string> callback = null)
        {
            if (_isLoading)
            {
                return;
            }
            else
            {
                _isLoading = true;
            }
            _inputService.Disable();
            await _sceneTransition.StartAnimation();
            DI.RemoveAllForThisScene();
            await _sceneLoader.LoadBase(nextScene, _sceneTransition, async (loadingScene) =>
            {
                for (float k = 0.8f; k < 0.9f; k += 0.01f)
                {
                    await UniTask.Delay(10);
                    _sceneTransition.SetProgress(k);
                }
                await InitializeAndSwapToNewScene(nextScene, loadingScene);

            });
        }

        private async UniTask InitializeAndSwapToNewScene(string nextScene, Scene LoadingScene)
        {
            SceneManager.SetActiveScene(LoadingScene);
            await _stateMachine.Enter<LoadLevelState, string>(nextScene);
            for (float k = 0.9f; k < 1f; k += 0.01f)
            {
                await UniTask.Delay(10);
                _sceneTransition.SetProgress(k);
            }
            GC.Collect();
            await _sceneTransition.EndAnimation();
            OnSceneLoaded?.Invoke();
            _inputService.Enable();
            _isLoading = false;
        }

        public bool IsLoading() =>
            _isLoading;

    }


}

