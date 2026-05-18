using Assets.Project.CodeBase.StaticData.Field;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Assets.Project.CodeBase.StaticData
{
    public interface IStaticDataService
    {
        UniTask<T> LoadAsset<T>(AssetReference reference) where T : class;
        UniTask LoadBaseAssets();
    }
}
