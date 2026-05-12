
using Assets.Project.CodeBase.Data;
using Assets.Project.CodeBase.Data.Progress;
using Assets.Project.CodeBase.Infostructure.Services.ProgressService;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure.Services.SaveService
{
    public class SaveService : ISaveService
    {
        private const string SAVE_SLOT_NAME = "Save";
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IProgressService _progressService;
        public SaveService(IProgressService progressService) =>
            _progressService = progressService;

        public void RegisterWriter(ISavedProgress writer) =>
            ProgressWriters.Add(writer);
        public void RemoveWriter(ISavedProgress writer) =>
            ProgressWriters.Remove(writer);
        public void Save()
        {
            foreach (ISavedProgress reader in ProgressWriters)
            {
                reader.UpdateProgress(_progressService.Progress);
            }
            string item = JsonUtility.ToJson(_progressService.Progress);
            PlayerPrefs.SetString(SAVE_SLOT_NAME, item);
        }
        public void Load()
        {
            string item = PlayerPrefs.GetString(SAVE_SLOT_NAME);
            if (item != null && item != default)
            {
                PlayerProgress progress = JsonUtility.FromJson<PlayerProgress>(item);
                if (progress != null)
                {
                    _progressService.Progress.SetFromSave(progress);
                }
            }
        }


    }


}
