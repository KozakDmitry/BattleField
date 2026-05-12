using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.Logic.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure.States
{
    /// <summary>
    /// Здесь обычно только инициализация сцены, этот скрипт вызывается при каждой загрузке сцены(кроме сцены самой загрузки)
    /// </summary>
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private SetupController _controller;
        public LoadLevelState(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;
        public async UniTask Enter(string sceneName) =>
            await LoadDependencies();

        private async UniTask LoadDependencies()
        {
            _controller = await DI.ResolveAsync<SetupController>();
            await _controller.Initialize();
            _stateMachine.Enter<GameLoopState>();
        }



        public void Exit()
        {

        }
    }
}