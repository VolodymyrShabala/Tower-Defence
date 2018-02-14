using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingNode : MonoBehaviour {
    [SerializeField] private Material hoverOverMat;
    private Material normalMat;
    private Renderer _renderer;
    private bool busy;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        normalMat = _renderer.material;
    }

    private void OnMouseOver() {
        if (EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        _renderer.material = hoverOverMat;
    }

    private void OnMouseExit() {
        _renderer.material = normalMat;
    }

    //Make an input manager to handle all inputs? Maybe that wil help to close choose tower menu if pressed somewhere else
    private void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        if (busy){
            return;
        }
        GameManager.buildingManager.OnNodeClick(this);
    }

    public bool Busy {
        set {
            busy = value;
        }
    }
}