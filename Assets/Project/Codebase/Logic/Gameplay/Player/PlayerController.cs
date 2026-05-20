using Assets.Project.CodeBase.Infostructure.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;


        private bool _isInit = false;

        private Rigidbody _rb;
        private IInputService _inputService;
        private Vector2 _moveDirection;
        public void Initialize()
        {
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

        private void FixedUpdate()
        {
            if (_isInit)
            {
                _rb.angularVelocity = new Vector2(_moveDirection.x * _speed, _moveDirection.y * _speed);
            }
        }
    }
}
