using UnityEngine;

public class PathRevealer : MonoBehaviour {
    private Transform[] childs;

    private void OnDrawGizmos() {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            childs[i] = transform.GetChild(i);
        }

        Gizmos.color = Color.green;
        foreach (Transform child in childs) {
            Gizmos.DrawWireSphere(child.transform.position, 0.1f);
        }
    }

}