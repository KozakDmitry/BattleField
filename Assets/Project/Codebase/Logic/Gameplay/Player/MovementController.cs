using Assets.Project.CodeBase.Infostructure.Input;
using Assets.Project.Codebase.StaticData;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private bool _isLookForward;

        private MovementConfig _config;
        private bool _isInit = false;
        private bool _isRunning = false;
        private float _currentVitality;

        private Transform _cameraTransform;
        private Rigidbody _rb;
        private IInputService _inputService;
        private Vector2 _moveDirection;

        public void Initialize(Transform cameraTransform, MovementConfig config)
        {
            _cameraTransform = cameraTransform;
            _config = config;
            SetBaseValues();
            _inputService = DI.ResolveSync<IInputService>();
            _inputService.OnMoveEvent += UpdateMoveDirection;
            _inputService.OnShiftEvent += UpdateRunningState;
            _isInit = true;
        }

        private void SetBaseValues()
        {
            _rb = GetComponent<Rigidbody>();
            _moveDirection = new Vector2(0, 0);
            _currentVitality = _config.MaxVitality;
        }

        private void UpdateMoveDirection(Vector2 value)
        {
            _moveDirection = value;
        }

        private void UpdateRunningState(bool isPressed)
        {
            if (isPressed && _currentVitality <= 0f)
                return;

            _isRunning = isPressed;
        }

        private void HandleVitality()
        {
            if (_isRunning && _currentVitality > 0f)
            {
                _currentVitality -= _config.VitalityDrainRate * Time.fixedDeltaTime;

                if (_currentVitality <= 0f)
                {
                    _currentVitality = 0f;
                    _isRunning = false;
                }
            }
            else if (_currentVitality < _config.MaxVitality)
            {
                _currentVitality += _config.VitalityRegenRate * Time.fixedDeltaTime;

                if (_currentVitality > _config.MaxVitality)
                    _currentVitality = _config.MaxVitality;
            }
        }

        public void CleanUp()
        {
            if (_inputService != null)
            {
                _inputService.OnMoveEvent -= UpdateMoveDirection;
                _inputService.OnShiftEvent -= UpdateRunningState;
            }
        }

        private Vector3 _tempForward,
                        _tempRight;

        private void FixedUpdate()
        {
            if (!_isInit)
                return;

            HandleVitality();

            float speed = _isRunning && _currentVitality > 0f ? _config.RunSpeed : _config.WalkSpeed;

            _tempForward = _cameraTransform.forward;
            _tempRight = _cameraTransform.right;

            _tempForward.y = 0f;
            _tempRight.y = 0f;
            _tempForward.Normalize();
            _tempRight.Normalize();
            Vector3 move = _tempForward * _moveDirection.y + _tempRight * _moveDirection.x;
            _characterController.Move(speed * Time.fixedDeltaTime * move);

            if (_isLookForward && move.sqrMagnitude > 0.001f)
            {
                Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10 * Time.fixedDeltaTime);
            }

        }
    }
}
