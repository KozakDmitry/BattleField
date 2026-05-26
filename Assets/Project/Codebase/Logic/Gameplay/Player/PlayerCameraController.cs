using Assets.Project.Codebase.Logic.Gameplay.Cam;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class PlayerCameraController
    {
        private ICameraFollow _cameraFollow;
        private PlayerController _playerTransform;

        public Transform CameraTransform => _cameraFollow.CameraTransform;

        public void Initialize(PlayerController controller)
        {
            _playerTransform = controller;
            _cameraFollow = DI.ResolveSync<ICameraFollow>();
            _cameraFollow.FollowTarget(_playerTransform.transform);
        }
    }
}
