using System.Collections.Generic;
using Generated.Addressables;
using Odin.Serializer.OdinSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AddressablesGroupEntry
{
    public AddressablesGroup addressableGroup;
    public AssetReference assetReference;
}

[CreateAssetMenu(menuName = "Addressables/Group Mapping")]
public class AddressablesGroupMapping : SerializedScriptableObject
{
    [SerializeField] private Dictionary<AddressablesAssetName, AddressablesGroupEntry> m_Entries = new();

    public AddressablesGroupEntry GetEntry(AddressablesAssetName name)
    {
        m_Entries ??= new Dictionary<AddressablesAssetName, AddressablesGroupEntry>();
        m_Entries.TryGetValue(name, out var entry);
        return entry;
    }

    public AssetReference GetReference(AddressablesAssetName name)
    {
        return GetEntry(name)?.assetReference;
    }
}
