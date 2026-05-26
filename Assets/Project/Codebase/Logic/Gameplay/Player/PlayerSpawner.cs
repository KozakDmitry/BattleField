using Assets.Project.Codebase.StaticData;
using Assets.Project.CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class PlayerSpawner : IPlayerSpawner
    {
        private PlayerController _playerInstance;

        public PlayerController PlayerInstance => _playerInstance;

        public async UniTask Spawn(Vector3 position)
        {
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(AddressablesNames.Player);

            if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
                position.y = hit.point.y;

            _playerInstance = Object.Instantiate(prefab, position, Quaternion.identity).GetComponent<PlayerController>();

            PlayerCameraController cameraController = new PlayerCameraController();
            cameraController.Initialize(_playerInstance);
            _playerInstance.Initialize(cameraController.CameraTransform);
        }

        public void CleanUp()
        {
            if (_playerInstance != null)
                _playerInstance.CleanUp();
        }
    }
}
