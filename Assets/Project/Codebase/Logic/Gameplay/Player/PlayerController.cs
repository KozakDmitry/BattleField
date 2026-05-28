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
        [SerializeField] private bool _isLookForward;

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

            _tempForward.y = 0f;
            _tempRight.y = 0f;
            _tempForward.Normalize();
            _tempRight.Normalize();
            Vector3 move = _tempForward * _moveDirection.y + _tempRight * _moveDirection.x;
            _characterController.Move(_speed * Time.fixedDeltaTime * move);

            if (_isLookForward && move.sqrMagnitude > 0.001f)
            {
                Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10 * Time.fixedDeltaTime);
            }


            //Vector3 move = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            //_characterController.Move(_speed * Time.deltaTime * move);
        }
    }
}
