using Assets.Project.Codebase.Logic.Shared.Cam;
using Assets.Project.CodeBase.Logic.Shared;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Cam
{
    public class CameraFollow : InitializableWindow, ICameraFollow
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -8f);
        [SerializeField] private float _followDamping = 1f;

        private VirtualCameraInstaller _vcam;

        public override async UniTask Initialize()
        {
            _vcam = await DI.ResolveAsync<VirtualCameraInstaller>();
            _vcam.CameraOffset.Offset = _offset;
            DI.Register<ICameraFollow>(this, mode: RegisterMode.scene);
        }

        public void FollowTarget(Transform target)
        {
            _vcam.CinemachineCamera.Follow = target;
            _vcam.CinemachineCamera.LookAt = target;
        }
    }
}
