using Assets.Project.CodeBase.StaticData.Field;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;

namespace Assets.Project.CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {

        public async UniTask LoadBaseAssets()
        {

        }

        public async UniTask<T> LoadAsset<T>(AssetReference reference) where T : class
        {
            if (reference is null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            return await Addressables.LoadAssetAsync<T>(reference);
        }

    }
}
