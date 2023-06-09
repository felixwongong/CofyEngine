﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace cofydev.util.Editor
{
public class SceneSelectorWindow : EditorWindow
{
    private static List<string> scenePaths;
    private Vector2 scrollPosition;
    private static SceneSelectorWindow _window;

    [MenuItem("Window/Scene Selector #s")]
    internal static void ShowWindow()
    {
        if (_window != null) _window.Close();
        else
        {
            _window = GetWindow<SceneSelectorWindow>(true, "Scene Selector", true);
            _window.minSize = new Vector2(200, 300);
            _window.maxSize = new Vector2(200, 300);
            RefreshSceneList();
        }
    }

    private void OnGUI()
    {
        if (scenePaths == null || scenePaths.Count == 0) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (string scenePath in scenePaths)
        {
            if (GUILayout.Button(Path.GetFileNameWithoutExtension(scenePath), EditorStyles.miniButton))
            {
                OnSceneButtonClick(scenePath);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private static void OnSceneButtonClick(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            if(_window != null) _window.Close();
        }
    }

    private static void RefreshSceneList()
    {
        scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenePaths.Add(scene.path);
            }
        }
    }
}
}