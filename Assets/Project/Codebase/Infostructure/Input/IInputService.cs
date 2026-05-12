using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.StaticData.Input;

namespace Assets.Project.CodeBase.Infostructure.Input
{
    public interface IInputService
    {
        event InputService.StartT OnStartEvent;
        event InputService.EndT OnEndEvent;

        InputConfigData GetInputConfig();
        void Disable();
        void Enable();
    }
}