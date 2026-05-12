

using Assets.Project.CodeBase.StaticData;
using Assets.Project.CodeBase.StaticData.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.Project.CodeBase.Infostructure.Input
{
    public class InputService : IInputService
    {

        public delegate void StartT(Vector2 position, float time);
        public event StartT OnStartEvent;
        public delegate void EndT(Vector2 position, float time);
        public event EndT OnEndEvent;

        private TouchControls _touchControls;
        private InputConfigData _inputSettings;
        private IStaticDataService _staticDataService;
        public InputService(IStaticDataService staticData)
        {
            _staticDataService = staticData;
            _inputSettings = _staticDataService.ForInputConfig();
            _touchControls = new TouchControls();
            _touchControls.Enable();
            RegisterEvents();
        }
        public InputConfigData GetInputConfig() => 
            _inputSettings;

        private void RegisterEvents()
        {
            _touchControls.Player.Press.started += ctx => StartTouch(ctx);
            _touchControls.Player.Press.canceled += ctx => EndTouch(ctx);
        }

        private void EndTouch(InputAction.CallbackContext ctx)
        {
            if (OnEndEvent != null)
            {
                OnEndEvent?.Invoke(_touchControls.Player.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
            }
        }

        private void StartTouch(InputAction.CallbackContext ctx)
        {
            if (OnStartEvent != null)
            {
                OnStartEvent?.Invoke(_touchControls.Player.TouchPosition.ReadValue<Vector2>(), (float)ctx.startTime);
            }
        }

        public void Disable() =>
            _touchControls.Disable();

        public void Enable() =>
            _touchControls.Enable();


    }
}
