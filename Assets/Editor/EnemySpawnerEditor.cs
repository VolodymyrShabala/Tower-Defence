using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if(GUILayout.Button("Spawn Wave", GUILayout.Width(300))) {
            OpenSpawnWindow();
        }
    }

    private void OpenSpawnWindow() {
        EnemySpawnerEditorWindow.Init();
    }
}