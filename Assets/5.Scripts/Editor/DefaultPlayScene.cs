﻿using UnityEditor;
using UnityEditor.SceneManagement;

namespace _5.Scripts.Editor
{
    public static class DefaultPlayScene
    {
        [InitializeOnLoadMethod]
        private static void SetDefaultPlayScene()
        {
            if (EditorBuildSettings.scenes.Length == 0) return;
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        }
    }
}