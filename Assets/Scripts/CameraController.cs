using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
    [SerializeField] private Vector2 minMaxZoom = new Vector2(5, 15);
    [SerializeField] private int dragSpeed = 5;

    private void Start() {
        FindObjectOfType<MapGenerator>().mapCreated += AdjustmentForNewPlane;
    }

    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        Zoom();
        Drag();
    }

    private void Zoom() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0){
            transform.position += Vector3.down;
            if (transform.position.y < minMaxZoom.x){
                transform.position = new Vector3(transform.position.x, minMaxZoom.x, transform.position.z);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0){
            transform.position += Vector3.up;
            if (transform.position.y > minMaxZoom.y){
                transform.position = new Vector3(transform.position.x, minMaxZoom.y, transform.position.z);            }
        }
    }

    private void Drag() {
        if (Input.GetMouseButton(2)){
            float vertical = Input.GetAxis("Mouse Y");
            transform.position -= new Vector3(0, 0, vertical) * Time.deltaTime * dragSpeed;
        }
    }

    public void AdjustmentForNewPlane(float y) {

        transform.position = new Vector3(y / 2, transform.position.y > y? transform.position.y : y, y / 2);
        minMaxZoom.y = y + 30;
    }
}