using UnityEngine;

[RequireComponent(typeof(ObjectPoolingManager))]
public class ObjectManager : MonoBehaviour {
    /* When adding a new bullet or enemy don't forget to add it in the enum in the bottom of the script
     * also add it in switch case in GetBullet or GetEnemy */

    [Header("Bullets type")]
    public Bullet simple;
    public Bullet splash;

    [Header("Enemy types")]
    public Enemy simpleEnemy;
    public Enemy fast;
    public Enemy flyingEnemy;
    public Enemy boss;

    public Enemy GetEnemy(EnemyType t) {
        if (!System.Enum.IsDefined(typeof(EnemyType), t)) {
            Debug.LogError("There is no such enemy in Enemy Type Enum in Object Manager script");
            return null;
        }

        switch (t) {
            case EnemyType.Boss:
                return boss;
            case EnemyType.FlyingEnemy:
                return flyingEnemy;
            case EnemyType.Fast:
                return fast;
            case EnemyType.Simple:
                return simpleEnemy;
            default:
                Debug.LogError("Something went wrong in Object Manager");
                return null;
        }
    }

    public Bullet GetBullet(BulletType b) {
        if (!System.Enum.IsDefined(typeof(BulletType), b)) {
            Debug.LogError("There is no such bullet in Bullet Type enum in Object Manager script");
            return null;
        }

        switch (b) {
            case BulletType.Simple:
                return simple;
            case BulletType.Splash:
                return splash;
            default:
                Debug.LogError("Something went wrong in Object Manager");
                return null;
        }
    }
}

public enum BulletType {
    Simple = 0,
    Splash = 1
}

public enum EnemyType {
    Simple,
    Fast,
    FlyingEnemy,
    Boss
}