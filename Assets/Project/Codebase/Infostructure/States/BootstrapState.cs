using Assets.Project.CodeBase.Extentions;
using Assets.Project.CodeBase.Infostructure.Input;
using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.Infostructure.Services.ProgressService;
using Assets.Project.CodeBase.Infostructure.Services.SaveService;
using Assets.Project.CodeBase.Infostructure.Services.SceneService;
using Assets.Project.CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Project.CodeBase.Infostructure.States
{
    /// <summary>
    /// Инициализация всех сервисов(не монобехов) 
    /// </summary>
    public class BootstrapState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private const SceneNames.Name Initial = SceneNames.Name.Start;
        private IProgressService _progressService;
        private IStaticDataService _staticDataService;
        public BootstrapState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            RegisterServices();
        }

        private void RegisterInnerServices()
        {
            _progressService = new ProgressService();
            RegisterStaticData();
        }

        public void RegisterServices()
        {
            RegisterInnerServices();
            RegisterStaticData();
            RegisterDataServices();
            DI.Register<IInputService>(new InputService(_staticDataService));
            DI.Register<ISaveService>(new SaveService(_progressService));
            DI.Register<ISceneService>(new SceneService(_stateMachine,
                                                        DI.ResolveSync<IInputService>()));
        }


        private void RegisterDataServices()
        {
        }

        private void RegisterStaticData()
        {
            _staticDataService = new StaticDataService();
            _staticDataService.Load();
        }



        public async UniTask Enter(string startScene)
        {
            CheckStartScene(ref startScene);
            await DI.ResolveSync<ISceneService>().LoadFirstScene(SceneNames.GetSceneName(Initial));
            EnterLoadLevel(startScene);
        }

        private void CheckStartScene(ref string startScene)
        {
            if (startScene == SceneNames.GetSceneName(SceneNames.Name.Start))
            {
                startScene = SceneNames.GetSceneName(SceneNames.Name.GamePlay);
            }
        }

        private void EnterLoadLevel(string startScene) =>
              _stateMachine.Enter<LoadProgressState, string>(startScene);

        public void Exit()
        {

        }


    }

}