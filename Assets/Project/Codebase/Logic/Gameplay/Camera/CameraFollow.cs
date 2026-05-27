using Assets.Project.Codebase.Logic.Shared.Cam;
using Assets.Project.CodeBase.Infostructure.Input;
using Assets.Project.CodeBase.Logic.Shared;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Cam
{
    public class CameraFollow : InitializableWindow, ICameraFollow
    {
        [Title("Camera Settings")]
        [SerializeField]
        private float _zoomSpeed = 2f;
        [SerializeField]
        private float _zoomLerpSpeed = 10f;
        [SerializeField]
        private float _minDistance = 3f;
        [SerializeField]
        private float _maxDistance = 15f;


        private VirtualCameraInstaller _vcam;

        private Vector2 _scrollDelta;
        private float _targetZoom;
        private float _currentZoom;
        private bool _isInit = false;
        private IInputService _inputService;

        public Transform CameraTransform => 
            transform;
            

        public override async UniTask Initialize()
        {
            _inputService = DI.ResolveSync<IInputService>();
            _vcam = await DI.ResolveAsync<VirtualCameraInstaller>();
            _inputService.OnMouseZoom += UpdateZoom;
            Cursor.lockState = CursorLockMode.Locked;
            DI.Register<ICameraFollow>(this, mode: RegisterMode.scene);
            _isInit = true;
        }

        private void UpdateZoom(Vector2 position) =>
            _scrollDelta = position;



        private void FixedUpdate()
        {
            if (!_isInit)
            {
                return;
            }
            if (_scrollDelta.y != 0&& _vcam.OrbitalFollow!=null)
            {
                _targetZoom = Math.Clamp(_vcam.OrbitalFollow.Radius - _scrollDelta.y*_zoomSpeed, _minDistance, _maxDistance);
                _scrollDelta = Vector2.zero;
            }

            _currentZoom = Mathf.Lerp(_currentZoom,_targetZoom, Time.deltaTime*_zoomLerpSpeed);
            _vcam.OrbitalFollow.Radius = _currentZoom;
        }

        public void FollowTarget(Transform target) =>
            _vcam.CinemachineCamera.Follow = target;
    }
}
