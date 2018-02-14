using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectManager))]
public class ObjectPoolingManager : MonoBehaviour {
    private Dictionary<BulletType, List<Bullet>> bulletsObjectPolling;
    private Dictionary<EnemyType, List<Enemy>> enemyObjectPolling;

    public static ObjectPoolingManager instance;

    private ObjectManager om;


    //FIXME: Create some objects in the start
    private void Awake() {
        instance = this;
        bulletsObjectPolling = new Dictionary<BulletType, List<Bullet>>();
        enemyObjectPolling = new Dictionary<EnemyType, List<Enemy>>();
        CreateDictionaries();
        om = FindObjectOfType<ObjectManager>();
        if (om == null) {
            Debug.LogError("There is always must be an Object playerManager!");
        }
    }

    private void CreateDictionaries() {
        Array x = Enum.GetValues(typeof(BulletType));
        foreach (object o in x){
            bulletsObjectPolling.Add((BulletType)o, new List<Bullet>());
        }

        Array z = Enum.GetValues(typeof(EnemyType));
        foreach (object o in z){
            enemyObjectPolling.Add((EnemyType)o, new List<Enemy>());
        }
    }

    public Enemy SpawnEnemy(EnemyType _e, Transform t) {
        if (enemyObjectPolling.ContainsKey(_e)) {
            List<Enemy> enemyList = enemyObjectPolling[_e];
            foreach (Enemy e in enemyList){
                if (!e.gameObject.activeInHierarchy){
                    e.gameObject.SetActive(true);
                    return e;
                }
            }
            Enemy spawnEnemy = Instantiate(om.GetEnemy(_e), t);
            enemyList.Add(spawnEnemy);
            return spawnEnemy;
        }

        Debug.LogError("There is no such enemy in the list. Check enum");
        return null;
    }

    public Bullet Shoot(BulletType b, Transform t) {
        if (bulletsObjectPolling.ContainsKey(b)){
            List<Bullet> bullet = bulletsObjectPolling[b];
            foreach (Bullet o in bullet){
                if (!o.gameObject.activeInHierarchy){
                    o.gameObject.SetActive(true);
                    return o;
                }              
            }
            Bullet bt = Instantiate(om.GetBullet(b), t.position, Quaternion.identity, transform);
            bullet.Add(bt);
            return bt;
        }
        Debug.LogError("Unknown bullet type. Check Enum with bullet types.");
        return null;
    }
}