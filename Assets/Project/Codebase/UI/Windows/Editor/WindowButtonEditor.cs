using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Common.DiffMergeToolConfig;

[CustomEditor(typeof(WindowButton))]
public class WindowButtonEditor : Editor
{
    private SerializedProperty _WindowName;
    private List<string> WNames = new List<string>();
    private List<string> WNamesForDraw = new List<string>();
    private const string kNoWindow = "<None>";

    Window[] windows;

    public void OnEnable()
    {
        _WindowName = serializedObject.FindProperty("WindowName");
        windows = FindObjectsByType<Window>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    void FindAllWindow(){
        foreach (var item in windows)
        {
            WNamesForDraw.Add( item.Name );
            WNames.Add( item.Name);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WindowButton button = (WindowButton)target;

        serializedObject.Update();

        EditorGUILayout.LabelField(new GUIContent("Window:", "Выбор нужного окна. Если не указать, в режиме закрытия будет закрыто последнее окно"));

        WNames.Clear();
        WNames.Add( kNoWindow );

        WNamesForDraw.Clear();
        WNamesForDraw.Add( kNoWindow );

        FindAllWindow();

        int currentIndex = string.IsNullOrEmpty(button.WindowName) ? 0 : WNames.IndexOf(button.WindowName);

        int newIndex = EditorGUILayout.Popup(currentIndex, WNamesForDraw.ToArray());
        if (newIndex > 0 && newIndex < WNames.Count) {
            _WindowName.stringValue = WNames[newIndex];
        } else {
            _WindowName.stringValue = string.Empty;
        }

        
        

        serializedObject.ApplyModifiedProperties();
    }
}
