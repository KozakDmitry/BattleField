using System.Threading;
using Assets.Project.CodeBase.Infostructure.Services.SaveService;
using Assets.Project.CodeBase.Infostructure.Services.SceneService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure.States
{
    /// <summary>
    /// Здесь обычно инициализация настроек, вроде сеток и остального, инициализируется 1 раз в самом начале
    /// </summary>
    public class LoadProgressState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneService _sceneService;
        private readonly ISaveService _saveService;
        public LoadProgressState(IGameStateMachine gameStateMachine, ISceneService sceneService, ISaveService saveService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneService = sceneService;
            _saveService = saveService;
        }

        public async UniTask Enter(string startScene)
        {
            Application.targetFrameRate = 60;
            _saveService.Load();
            await _sceneService.LoadScene(startScene);
        }

        public void Exit()
        {

        }
    }
}