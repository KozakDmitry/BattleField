using Assets.Project.CodeBase.Data.Progress.SaveData;
using Assets.Project.CodeBase.Infostructure.Services.SaveSystem;
using Assets.Project.Codebase.Logic.Gameplay.Cam;
using Assets.Project.Codebase.Logic.Gameplay.Field.SpawnPoints;
using Assets.Project.Codebase.Logic.Gameplay.Player;
using Assets.Project.Codebase.StaticData;
using Assets.Project.CodeBase.Logic.Shared;
using Assets.Project.CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public class FieldBuilder : InitializableWindow, IFieldBuilder
    {
        private IField _field;
        private IPlayerSpawner _playerSpawner;
        private ISaveSystem _saveSystem;
        private int _currentLevel;

        public override async UniTask Initialize()
        {
            _field = GetComponent<IField>();
            _saveSystem = DI.ResolveSync<ISaveSystem>();
            _playerSpawner = new PlayerSpawner();

            await BuildField();
        }

        private void CheckCurrentLevel()
        {
            var levelData = _saveSystem.Load<LevelSaveData>("Level");
            _currentLevel = levelData?.currentLevel ?? 1;
        }

        public async UniTask BuildField()
        {
            CheckCurrentLevel();
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(AddressablesNames.Field + _currentLevel);
            _field.Spawn(prefab);
            await SpawnPlayer();
        }

        private async UniTask SpawnPlayer()
        {
            await _playerSpawner.Spawn(_field.GetSpawnPosition());

        }


        public override void CleanUp()
        {
            _playerSpawner?.CleanUp();
            base.CleanUp();
        }
    }
}
