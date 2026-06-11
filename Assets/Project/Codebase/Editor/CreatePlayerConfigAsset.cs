using Assets.Project.Codebase.StaticData;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Assets.Project.Codebase.Editor
{
    public static class CreatePlayerConfigAsset
    {
        private const string PlayerConfigPath = "Assets/Project/StaticData/PlayerConfig/PlayerConfig.asset";
        private const string MovementConfigPath = "Assets/Project/StaticData/PlayerConfig/MovementConfig.asset";

        [MenuItem("Tools/Create Player Config Asset")]
        public static void Create()
        {
            PlayerConfig existing = AssetDatabase.LoadAssetAtPath<PlayerConfig>(PlayerConfigPath);
            if (existing != null)
            {
                Debug.Log("PlayerConfig already exists at " + PlayerConfigPath);
                Selection.activeObject = existing;
                return;
            }

            string directory = System.IO.Path.GetDirectoryName(PlayerConfigPath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            MovementConfig movementConfig = ScriptableObject.CreateInstance<MovementConfig>();
            AssetDatabase.CreateAsset(movementConfig, MovementConfigPath);

            PlayerConfig playerConfig = ScriptableObject.CreateInstance<PlayerConfig>();
            AssetDatabase.CreateAsset(playerConfig, PlayerConfigPath);

            SerializedObject so = new SerializedObject(playerConfig);
            SerializedProperty prop = so.FindProperty("_movementConfig");
            prop.objectReferenceValue = movementConfig;
            so.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings != null)
            {
                string groupName = "Configs";
                AddressableAssetGroup group = settings.FindGroup(groupName);
                if (group == null)
                    group = settings.CreateGroup(groupName, false, false, false, null);

                string guid = AssetDatabase.AssetPathToGUID(PlayerConfigPath);
                settings.CreateOrMoveEntry(guid, group, false, false);
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
            }

            Selection.activeObject = playerConfig;
            Debug.Log("PlayerConfig created with MovementConfig reference at " + PlayerConfigPath);
        }
    }
}
