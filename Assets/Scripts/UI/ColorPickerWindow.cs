using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPickerWindow : MonoBehaviour {
    private BaseEventData b;
    private bool followMouse;

    private void Update() {
        if (followMouse) {
            Follow();
        }
    }

    private void Follow() {
        transform.position = Input.mousePosition;
    }

    public void DiactivateWindow() {
        gameObject.SetActive(false);
    }

    public void PointerDown() {
        followMouse = true;
    }

    public void PointerUp() {
        followMouse = false;
    }
}