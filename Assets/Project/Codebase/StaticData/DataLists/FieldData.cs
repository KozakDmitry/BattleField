using Odin.Serializer.OdinSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Project.Codebase.StaticData.DataLists
{
    [CreateAssetMenu(fileName = "FieldData", menuName = "StaticData/FieldData")]
    public class FieldData : SerializedScriptableObject
    {
        public Dictionary<int, AssetReference> levels;
        public Dictionary<int, AssetReference> players;
    }
}
