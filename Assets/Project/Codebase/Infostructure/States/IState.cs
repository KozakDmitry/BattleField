using Cysharp.Threading.Tasks;

namespace Assets.Project.CodeBase.Infostructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }



    public interface IPayloadedState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);

    }
    public interface IExitableState
    {
        void Exit();
    }


}