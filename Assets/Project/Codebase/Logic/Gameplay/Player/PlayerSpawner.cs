using Assets.Project.CodeBase.Logic.Shared;
using Assets.Project.CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public class PlayerSpawner : InitializableWindow
    {
        [SerializeField] private AssetReference _playerReference;
        [SerializeField] private Vector3 _spawnPosition;

        private PlayerController _playerInstance;

        public PlayerController PlayerInstance => _playerInstance;

        public async override UniTask Initialize()
        {
            await Spawn();
        }
        public async UniTask Spawn()
        {
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(_playerReference);
            _playerInstance = Object.Instantiate(prefab, _spawnPosition, Quaternion.identity).GetComponent<PlayerController>();
            _playerInstance.Initialize();
        }

        public override void CleanUp()
        {
            if (_playerInstance != null)
            {
                _playerInstance.CleanUp();
            }
            base.CleanUp();
        }
    }
}
