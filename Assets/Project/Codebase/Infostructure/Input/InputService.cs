

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.Project.CodeBase.Infostructure.Input
{
    public class InputService : IInputService
    {

        public delegate void Move(Vector2 position);
        public event Move OnMoveEvent;
        public delegate void EndT(Vector2 position);
        public event EndT OnEndEvent;
        public delegate void OnMouseZ(Vector2 position);
        public event OnMouseZ OnMouseZoom;
        public delegate void Shift(bool isPressed);
        public event Shift OnShiftEvent;

        private TouchControls _touchControls;
        public InputService()
        {
            _touchControls = new TouchControls();
            _touchControls.Enable();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _touchControls.PC.Move.performed += OnMove;
            _touchControls.PC.Move.canceled += OnEndMove;
            _touchControls.PC.Shift.performed += OnShift;
            _touchControls.PC.Shift.canceled += OnShift;
            RegisterCameraEvents();
        }

        private void RegisterCameraEvents()
        {
            _touchControls.PC.MouseZoom.performed += OnMouseZoomed;
        }

        private void OnMouseZoomed(InputAction.CallbackContext context) =>
            OnMouseZoom?.Invoke(context.ReadValue<Vector2>());

        private void OnMove(InputAction.CallbackContext context) => 
            OnMoveEvent?.Invoke(_touchControls.PC.Move.ReadValue<Vector2>());


        private void OnEndMove(InputAction.CallbackContext context) =>
            OnEndEvent?.Invoke(_touchControls.PC.Move.ReadValue<Vector2>());

        private void OnShift(InputAction.CallbackContext context) =>
            OnShiftEvent?.Invoke(context.performed);

        public void Disable() =>
            _touchControls.Disable();

        public void Enable() =>
            _touchControls.Enable();


    }
}
