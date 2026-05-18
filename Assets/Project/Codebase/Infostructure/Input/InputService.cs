

using Assets.Project.CodeBase.StaticData;
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
        private IStaticDataService _staticDataService;
        public InputService(IStaticDataService staticData)
        {
            _staticDataService = staticData;
            _touchControls = new TouchControls();
            _touchControls.Enable();
            RegisterEvents();
        }

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
