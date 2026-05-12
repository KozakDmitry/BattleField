using Assets.Project.CodeBase.StaticData.Field;
using Assets.Project.CodeBase.StaticData.Input;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Project.CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string FIELD_CONFIG_PATH = "StaticData/Config/FieldConfig";
        private const string INPUT_CONFIG_PATH = "StaticData/Config/InputConfig";

        private const string TILES_DATA_PATH = "StaticData/LevelData/Tiles/TilesData";
        private const string PLAYER_DATA_PATH = "StaticData/LevelData/Player/PlayerData";
        private FieldConfigData _fieldConfigData;
        private InputConfigData _inputConfigData;
        public void Load()
        {
            LoadConfigData();
        }

      
        private void LoadConfigData()
        {
            _fieldConfigData = Resources.Load<FieldConfigData>(FIELD_CONFIG_PATH);
            _inputConfigData = Resources.Load<InputConfigData>(INPUT_CONFIG_PATH);
        }




        public FieldConfigData ForFieldConfig() =>
            _fieldConfigData;
        public InputConfigData ForInputConfig() =>
           _inputConfigData;

    }
}