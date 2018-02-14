using UnityEngine;

public class Tower : MonoBehaviour {
    [Tooltip("Do not assign if you do not want tower to rotate")]
    [SerializeField] private GameObject head;
    [SerializeField] private Transform placeToShootFrom;
    private Enemy closestEnemy;
    [SerializeField] private BulletType bulletType;

    [Header("Tower stats")]
    [SerializeField] private int damage = 1;
    [Tooltip("Shooting interval")]
    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private float bulletSpeed = 10;
    [Range(0, 3)]
    [SerializeField] private float splashRadius = 0;
    public int towerRange = 3;
    [SerializeField] private int rotateSpeed = 10;
    [SerializeField] private bool canAttackAir;
   
    [Header("Money")]
    [SerializeField] private int costToBuild = 5;
    [SerializeField] private int costToUpgrade = 10;

    private LayerMask enemyMask = ~7;

    private float timeHolder;

    private ObjectPoolingManager obm;

    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        FindClosestEnemy();
        timeHolder -= Time.deltaTime;
        if (timeHolder <= 0) {
            timeHolder = 0;
        }
        if (closestEnemy == null) {
            if (anim != null){
                anim.SetBool("Attacking", false);
            }
            return;
        }
        if(head == null) {
            return;
        }
        if (closestEnemy.AirUnit && !canAttackAir){
            return;
        }
        Vector3 vectorToTarget = closestEnemy.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(vectorToTarget);
        Vector3 rotation = Quaternion.Lerp(head.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime).eulerAngles;
        head.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        if (timeHolder <= 0) {
            Attack();
        }
    }

    private void Attack() {
        if (placeToShootFrom == null) {
            placeToShootFrom = transform;
        }
        if (anim != null){
            anim.SetBool("Attacking", true);
        }
        Bullet b = ObjectPoolingManager.instance.Shoot(bulletType, transform);
        if (b == null) {
            return;
        }
        //FIXME: I do not think this is neccesary to set at every bullet spawn. Maybe create bullet state?
        b.damage = damage;
        b.speed = bulletSpeed;
        b.splashRadius = splashRadius;
        b.target = closestEnemy;
        b.mask = enemyMask;
        timeHolder = attackRate;
    }

    //Change it to maybe enemy with less HP?
    private void FindClosestEnemy() {
        Collider[] enemies = Physics.OverlapSphere(transform.position, towerRange, enemyMask);
        if (enemies.Length == 0) {
            closestEnemy = null;
            return;
        }
        float distance = float.MaxValue;

        foreach (Collider enemy in enemies) {
            float newDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (newDistance < distance) {
                distance = newDistance;
                closestEnemy = enemy.GetComponent<Enemy>();
            }
        }
        if (closestEnemy != null && !closestEnemy.gameObject.activeInHierarchy) {
            closestEnemy = null;
        }
    }

    public int CostToBuild {
        get {
            return costToBuild;
        }
    }
    public int CostToUpgrade {
        get {
            return costToUpgrade;
        }
    }
}