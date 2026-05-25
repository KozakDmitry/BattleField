using Cysharp.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.CodeBase.StaticData
{
    public static class StaticDataService
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new();
        private static readonly ConcurrentDictionary<string, TaskCompletionSource<object>> _pendingLoads = new();



        public static async UniTask<T> LoadAsset<T>(AssetReference reference) where T : class
        {
            if (reference is null)
                throw new ArgumentNullException(nameof(reference));

            if (_cache.TryGetValue(reference.AssetGUID, out object cached))
                return (T)cached;

            var tcs = new TaskCompletionSource<object>();
            var existing = _pendingLoads.GetOrAdd(reference.AssetGUID, tcs);

            if (existing != tcs)
            {
                object result = await existing.Task;
                return (T)result;
            }

            try
            {
                T asset = await Addressables.LoadAssetAsync<T>(reference);
                _cache[reference.AssetGUID] = asset;
                tcs.TrySetResult(asset);
                return asset;
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                throw;
            }
            finally
            {
                _pendingLoads.TryRemove(reference.AssetGUID, out _);
            }
        }

        public static async UniTask<T> LoadAsset<T>(string assetId, Transform parent = null) where T : class
        {
            if (string.IsNullOrEmpty(assetId))
                throw new ArgumentNullException(nameof(assetId));

            if (_cache.TryGetValue(assetId, out object cached))
                return (T)cached;

            var tcs = new TaskCompletionSource<object>();
            var existing = _pendingLoads.GetOrAdd(assetId, tcs);

            if (existing != tcs)
            {
                object result = await existing.Task;
                return (T)result;
            }

            try
            {
                T asset = await Addressables.LoadAssetAsync<T>(assetId);
                _cache[assetId] = asset;
                tcs.TrySetResult(asset);
                return asset;
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                throw;
            }
            finally
            {
                _pendingLoads.TryRemove(assetId, out _);
            }
        }
    }


    public enum StaticDataCategory
    {
        Shared =0,
        Menu =1,
        Gameplay = 0,
    }
}
