using Assets.Project.CodeBase.Infostructure.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private CharacterController _characterController;


        private bool _isInit = false;

        private Transform _cameraTransform;
        private Rigidbody _rb;
        private IInputService _inputService;
        private Vector2 _moveDirection;
        public void Initialize(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
            SetBaseValues();
            _inputService = DI.ResolveSync<IInputService>();
            _inputService.OnMoveEvent += UpdateMoveDirection;
            _isInit = true;
        }

        private void SetBaseValues()
        {
            _rb = GetComponent<Rigidbody>();
            _moveDirection = new Vector2(0, 0);
        }

        private void UpdateMoveDirection(Vector2 value)
        {
            _moveDirection = value;
        }

        public void CleanUp()
        {
            if (_inputService != null)
            {
                _inputService.OnMoveEvent -= UpdateMoveDirection;
            }
        }


        private Vector3 _tempForward,
                        _tempRight;

        private void FixedUpdate()
        {
            if (!_isInit)
            {
                return;
            }
            _tempForward = _cameraTransform.forward;
            _tempRight = _cameraTransform.right;

            _tempForward.y = 0;
            _tempRight.y = 0;
            _tempForward.Normalize();
            _tempRight.Normalize();

            Vector3 moveDirection = _tempForward * _moveDirection.y + _tempRight * _moveDirection.x;
            _characterController.Move(moveDirection * _speed * Time.deltaTime);

        }
    }
}
