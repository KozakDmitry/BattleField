#if UNITY_EDITOR
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Assets.Project.Codebase.Extentions.Address
{
    public static class AddressablesLabelEnumGenerator
    {
        private const string OutputFileName = "AddressablesLabels.cs";

        [MenuItem("Tools/Addressables/Generate Labels to Enum")]
        public static void Generate()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null || settings.GetLabels() == null || settings.GetLabels().Count == 0)
            {
                Debug.LogWarning("⚠️ Addressables settings not found or no labels defined. Run Addressables Group window first.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("namespace Generated.Addressables");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// Auto-generated enum from Addressables labels.");
            sb.AppendLine("    /// Do not edit manually. Regenerate via Tools > Addressables > Generate Labels to Enum");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public enum AddressablesLabel");
            sb.AppendLine("    {");

            var usedNames = new System.Collections.Generic.HashSet<string>();
            foreach (var label in settings.GetLabels())
            {
                string cleanName = SanitizeLabel(label);
                // Защита от дубликатов после санитизации
                if (usedNames.Contains(cleanName))
                    cleanName = $"{cleanName}_dup{usedNames.Count}";

                usedNames.Add(cleanName);
                sb.AppendLine($"        {cleanName},");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            var guids = AssetDatabase.FindAssets("AddressablesLabelEnumGenerator");
            if (guids.Length == 0)
            {
                Debug.LogError("Could not locate AddressablesLabelEnumGenerator script.");
                return;
            }
            string outputFolder = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(guids[0]));
            string fullPath = Path.Combine(outputFolder, OutputFileName);
            Directory.CreateDirectory(outputFolder);
            File.WriteAllText(fullPath, sb.ToString());

            AssetDatabase.Refresh();
            Debug.Log($"✅ Successfully generated Addressables enum at {fullPath}");
        }

        private static string SanitizeLabel(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Unknown";

            // Заменяем всё, кроме букв, цифр и подчёркиваний
            string cleaned = Regex.Replace(raw, @"[^a-zA-Z0-9_]", "_");
            // Убираем двойные подчёркивания
            cleaned = Regex.Replace(cleaned, @"_+", "_");
            // Гарантируем начало с буквы (C# не разрешает начинать с цифры)
            if (char.IsDigit(cleaned[0])) cleaned = "_" + cleaned;
            // Убираем trailing _
            cleaned = cleaned.TrimEnd('_');
            if (string.IsNullOrEmpty(cleaned)) return "Unknown";
            return char.ToUpperInvariant(cleaned[0]) + cleaned.Substring(1);
        }
    }
#endif
}
