using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Enemy stats")]
    [SerializeField] private int health = 1;
    [SerializeField] private int moneyReward = 1;
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private bool airUnit;
    [Tooltip("If not assigned it will look after an object named Path")]
    [SerializeField] private GameObject path;

    private int healthHolder;

    private Transform[] childs;
    private int index;

    private void Start() {
        if (path == null) {
            path = GameObject.Find("Path");
        }
        childs = new Transform[path.transform.childCount];
        for (int i = 0; i < path.transform.childCount; i++) {
            childs[i] = path.transform.GetChild(i);
        }
        healthHolder = health;
    }

    private void Update() {
        Movement();
    }

    private void Movement() {
        if (index >= childs.Length) {
            ReachedGoal();
        }

        Vector3 dir = (childs[index].position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, childs[index].position) < 0.2f) {
            index++;
        }
    }

    public void TakeDamage(int damage) {
        if(healthHolder <= 0) {
            return;
        }

        healthHolder -= damage;

        if(healthHolder <= 0) {
            Die();
        }
    }

    private void ReachedGoal() {
        GameManager.playerManager.TakeDamage(damageToPlayer);
        gameObject.SetActive(false);
    }
    //Make fancy death animation
    private void Die() {
        GameManager.playerManager.AddMoney(moneyReward);
        gameObject.SetActive(false);
    }

    public void Reset() {
        index = 0;
        healthHolder = health;
    }

    public bool AirUnit {
        get {
            return airUnit;
        }
    }
}