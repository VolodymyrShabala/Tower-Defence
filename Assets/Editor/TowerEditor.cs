using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tower))]
public class TowerEditor : Editor {

    private void OnSceneGUI() {
        Tower tower = target as Tower;
        Handles.color = new Color(0, 1, 0, 0.1f);
        Handles.DrawSolidDisc(tower.transform.position, tower.transform.up, tower.towerRange);
    }
}