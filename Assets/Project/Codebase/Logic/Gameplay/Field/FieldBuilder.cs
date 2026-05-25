using Assets.Project.CodeBase.Data.Progress.SaveData;
using Assets.Project.CodeBase.Infostructure.Services.SaveSystem;
using Assets.Project.Codebase.Logic.Gameplay.Field.SpawnPoints;
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
        private ISaveSystem _saveSystem;
        private int _currentLevel;

        public override async UniTask Initialize()
        {
            _field = GetComponent<IField>();
            _saveSystem = DI.ResolveSync<ISaveSystem>();

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
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(AddressablesNames.Field + _currentLevel, null);
            _field.Spawn(prefab);

            FieldObject fieldObject = _field.FieldObject.GetComponent<FieldObject>();
            Vector3 spawnPosition = fieldObject.spawnPoint.GetSpawnPosition();

            await SpawnPlayer(spawnPosition);
        }

        private async UniTask SpawnPlayer(Vector3 position)
        {
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(AddressablesNames.Player, null);
            Object.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
