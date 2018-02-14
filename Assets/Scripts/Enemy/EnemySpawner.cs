using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public SpawnInfo[] spawnInfo;

    private float timeHolder;
    private int index;
    private int spawnedAmount;
    private bool doneSpawning;

    private void Start() {
        timeHolder = spawnInfo[index].timeBetweenWaves;
    }

    private void Update() {
        if (doneSpawning) {
            return;
        }
        if (timeHolder < 0) {
            timeHolder = spawnInfo[index].timeBetweenSpawns;
            SpawnEnemy();
        } else {
            timeHolder -= Time.deltaTime;
        }
    }

    private void SpawnNextWave() {
        
    }

    private void SpawnEnemy() {
        if(spawnInfo.Length == 0) {
            return;
        }
        Enemy enemy = ObjectPoolingManager.instance.SpawnEnemy(spawnInfo[index].enemyType, transform);
        enemy.transform.position = transform.position;
        enemy.Reset();
        spawnedAmount++;
        if (spawnedAmount == spawnInfo[index].spawnAmount) {
            index++;
            if (index == spawnInfo.Length) {
                doneSpawning = true;
                return;
            }
            timeHolder = spawnInfo[index].timeBetweenSpawns;
        }
    }

    [System.Serializable]
    public struct SpawnInfo {
        public EnemyType enemyType;
        public int spawnAmount;
        public float timeBetweenSpawns;
        public float timeBetweenWaves;
    }
}