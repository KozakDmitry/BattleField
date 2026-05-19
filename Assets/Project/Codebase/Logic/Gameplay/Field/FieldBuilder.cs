using Assets.Project.CodeBase.Logic.Shared;
using Assets.Project.CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public class FieldBuilder : InitializableWindow, IFieldBuilder
    {
        [SerializeField] private AssetReference _fieldReference;

        public override async UniTask Initialize()
        {
            await BuildField();
        }

        public async UniTask BuildField()
        {
            GameObject prefab = await StaticDataService.LoadAsset<GameObject>(_fieldReference);
            var field = new Field();
            field.Spawn(prefab);
        }
    }
}
