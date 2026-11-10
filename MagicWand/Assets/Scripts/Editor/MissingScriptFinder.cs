
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MissingScriptFinder : EditorWindow
{
    private List<GameObject> missingObjects = new List<GameObject>();

    [MenuItem("Tools/Find Missing Scripts")]
    public static void OpenWindow()
    {
        GetWindow<MissingScriptFinder>(
            "Missing Script Finder"
        );
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Search All Scenes"))
        {
            SearchMissingScripts();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Missing Scripts: " + missingObjects.Count
        );

        foreach (GameObject obj in missingObjects)
        {
            if (obj == null)
                continue;

            if (GUILayout.Button(
                obj.name,
                EditorStyles.linkLabel))
            {
                Selection.activeGameObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
        }
    }

    private void SearchMissingScripts()
    {
        missingObjects.Clear();

        GameObject[] allObjects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (EditorUtility.IsPersistent(obj))
                continue;

            if (obj.hideFlags != HideFlags.None)
                continue;

            int count =
                GameObjectUtility
                .GetMonoBehavioursWithMissingScriptCount(obj);

            if (count > 0)
            {
                missingObjects.Add(obj);
            }
        }

        Debug.Log(
            "Missing Scripts Found: " +
            missingObjects.Count
        );
    }
}
