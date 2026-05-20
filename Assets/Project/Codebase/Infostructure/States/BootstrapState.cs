using Assets.Project.CodeBase.Extentions;
using Assets.Project.CodeBase.Infostructure.Input;
using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.Infostructure.Services.ProgressService;
using Assets.Project.CodeBase.Infostructure.Services.SaveService;
using Assets.Project.CodeBase.Infostructure.Services.SceneService;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Project.CodeBase.Infostructure.States
{
    public class BootstrapState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private const SceneNames.Name Initial = SceneNames.Name.Start;

        public BootstrapState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            RegisterGlobalServices();
        }

        public async UniTask Enter(string startScene)
        {
            CheckStartScene(ref startScene);
            await DI.ResolveSync<ISceneService>().LoadFirstScene(SceneNames.GetSceneName(Initial));
            EnterLoadLevel(startScene);
        }

        private void RegisterGlobalServices()
        {
            ProgressService progressService = new ProgressService();

            DI.Register<IInputService>(new InputService());
            DI.Register<ISaveService>(new SaveService(progressService));
            DI.Register<ISceneService>(new SceneService(_stateMachine,
                                                        DI.ResolveSync<IInputService>()));
        }

        private void CheckStartScene(ref string startScene)
        {
            if (startScene == SceneNames.GetSceneName(SceneNames.Name.Start))
                startScene = SceneNames.GetSceneName(SceneNames.Name.GamePlay);
        }

        private void EnterLoadLevel(string startScene) =>
            _stateMachine.Enter<LoadProgressState, string>(startScene);

        public void Exit() { }
    }
}
