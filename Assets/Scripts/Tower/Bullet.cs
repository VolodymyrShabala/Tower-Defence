using UnityEngine;

public class Bullet: MonoBehaviour {
    public Enemy target;
    public float speed;
    public int damage;
    public float splashRadius;
    public LayerMask mask;

    //delete it later
    public bool visualliseSplashRadius;

    private bool show;

    private void Update() {
        if (target == null) {
            //If there is no target, what to do?
            //gameObject.SetActive(false);
            return;
        }
        Vector3 dir = target.transform.position - transform.position;
        transform.Translate(dir * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f) {
            ReachedTarget();
        }
    }

    private void ReachedTarget() {
        if (splashRadius > 0) {
            Collider[] col = Physics.OverlapSphere(transform.position, splashRadius, mask);
            foreach (Collider c in col) {
                c.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            show = true;
        }
        target.TakeDamage(damage);
        if (visualliseSplashRadius){
            Visuallise();
            return;
        }
        gameObject.SetActive(false);
    }

    //Delete this in the end
    private void Visuallise() {
        target = null;
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnDrawGizmos() {
        if (show) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}