using System.IO;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure.Services.SaveSystem
{
    public class JsonSaveSystem : ISaveSystem
    {
        public void Save<T>(string key, T data)
        {
            string path = Path.Combine(Application.persistentDataPath, key + ".json");
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }

        public T Load<T>(string key) where T : class
        {
            string path = Path.Combine(Application.persistentDataPath, key + ".json");
            if (!File.Exists(path))
                return null;

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
