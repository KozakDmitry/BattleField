using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.Scripts.Extentions.Files
{
    public static class FileHandler
    {

        public static void SaveToJSON<T>(List<T> toSave, string filename, bool overwrite)
        {
            Debug.Log(GetPath(filename));
            string content = JsonHelper.ToJson<T>(toSave.ToArray());
            WriteFile(GetPath(filename), content, overwrite);
        }

        public static void SaveToJSON<T>(T toSave, string filename, bool overwrite)
        {
            string content = JsonUtility.ToJson(toSave);
            WriteFile(GetPath(filename), content, overwrite);
        }

        public static List<T> ReadListFromJSON<T>(string filename)
        {
            string content = ReadFile(GetPath(filename));

            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                return new List<T>();
            }

            List<T> res = JsonHelper.FromJson<T>(content).ToList();

            return res;

        }

        public static T ReadFromJSON<T>(string filename)
        {
            string content = ReadFile(GetPath(filename));

            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                return default(T);
            }

            T res = JsonUtility.FromJson<T>(content);

            return res;

        }

        private static string GetPath(string filename)
        {
            return "Assets/Resources/StaticData/LevelData/Levels" + "/" + filename;
            //return Application.persistentDataPath + "/" + filename;
        }

        private static void WriteFile(string path, string content, bool overwrite)
        {
            FileMode mode = overwrite ? FileMode.Create : FileMode.Append;

            using (FileStream fileStream = new FileStream(path, mode, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                writer.Write(content);
            }
        }

        private static string ReadFile(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string content = reader.ReadToEnd();
                    return content;
                }
            }
            return "";
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
