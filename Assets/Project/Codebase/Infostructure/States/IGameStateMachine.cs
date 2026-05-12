
using Cysharp.Threading.Tasks;

namespace Assets.Project.CodeBase.Infostructure.States
{
    public interface IGameStateMachine
    {
        UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void Enter<TState>() where TState : class, IState;
    }
}
