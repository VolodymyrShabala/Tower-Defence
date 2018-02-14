using UnityEditor;
using UnityEngine;

public class EnemySpawnerEditorWindow : EditorWindow {
    private EnemySpawner enemySpawner;
    //change to a list?
    private static bool[] showMenu = new bool[10];
    //private string[] options = {"SimpleEnemy", "NotSimpleEnemy", "Boss"};
    //private int[] index = new int[showMenu.Length];
    private static EnemyType[] enemyTypes = new EnemyType[showMenu.Length];
    private static int[] spawnAmount = new int[showMenu.Length];
    private static float[] timeBetweenSpawns = new float[showMenu.Length];
    private static float[] timeBetweenWaves = new float[showMenu.Length];

    private GameObject g;
    private static Editor gameObjectEditor;
    private ObjectManager om;

    //FIXME: Every value resets if pressed
    public static void Init() {
        EditorWindow window = GetWindow(typeof(EnemySpawnerEditorWindow), false, "Spawner");
        window.position = new Rect(Screen.width, Screen.height / 3, 500, 500);
        window.Show();
    }

    private void OnGUI() {
        if (enemySpawner == null){
            enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner == null){
                Debug.LogError("Can't find Enemy Spawner object");
                return;
            }
        }
        if (om == null){
            om = FindObjectOfType<ObjectManager>();
        }
        //if button is pressed
        if (GUILayout.Button("New Wave")) {
            enemySpawner.spawnInfo = new EnemySpawner.SpawnInfo[enemySpawner.spawnInfo.Length + 1];
        }
        //field to write in how many wave are there
        int q = EditorGUILayout.DelayedIntField("Number of Waves", enemySpawner.spawnInfo.Length);
        enemySpawner.spawnInfo = new EnemySpawner.SpawnInfo[q];
        HasFoldoutBeenPressed();
        ClearEmptyPopUpMenues();
    }

    private void HasFoldoutBeenPressed() {
        for (int i = 0; i < enemySpawner.spawnInfo.Length; i++){
            showMenu[i] = EditorGUI.Foldout(GUILayoutUtility.GetRect(40, 40, 16, 16, EditorStyles.foldout), showMenu[i], "Wave: "  + (i + 1), true, EditorStyles.foldout);
            ShowInfo(i);
        }
    }

    private void ShowInfo(int index) {
        if (showMenu[index]){
            enemyTypes[index] = (EnemyType)EditorGUILayout.EnumPopup("Select Enemy", enemyTypes[index]);
            spawnAmount[index] = EditorGUILayout.IntField("Number of enemies: ", spawnAmount[index], EditorStyles.numberField);
            timeBetweenSpawns[index] = EditorGUILayout.FloatField("Time between each enemy spawn", timeBetweenSpawns[index], EditorStyles.numberField);
            timeBetweenWaves[index] = EditorGUILayout.FloatField("Time to next wave", timeBetweenWaves[index], EditorStyles.numberField);

            //Sends values back to enemy spawner
            enemySpawner.spawnInfo[index].enemyType = enemyTypes[index];
            enemySpawner.spawnInfo[index].spawnAmount = spawnAmount[index];
            enemySpawner.spawnInfo[index].timeBetweenSpawns = timeBetweenSpawns[index];
            enemySpawner.spawnInfo[index].timeBetweenWaves = timeBetweenWaves[index];

            //preview window of an enemy to spawn
            g = (GameObject)EditorGUILayout.ObjectField(om.GetEnemy(enemyTypes[index]).gameObject, typeof(GameObject), true);

            if (g != null){
                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(g);

                gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), GUIStyle.none);
            }
        }
    }

    private void ClearEmptyPopUpMenues() {
        for (int i = enemySpawner.spawnInfo.Length; i < showMenu.Length; i++) {
            showMenu[i] = false;
        }
    }

    //Why does this work? : It folds out menu even if I press on a name
    //private static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style) {
    //    Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
    //    // EditorGUI.kNumberW == 40f but is internal
    //    return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
    //}
    //private static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style) {
    //    return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
    //}
}

//public struct SpawnInfo {
//    public Enemy enemy;
//    public int spawnAmount;
//    public float timeBetweenSpawns;
//    public float timeBetweenWaves;
//}