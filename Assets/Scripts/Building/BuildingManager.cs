using UnityEngine;

public class BuildingManager : MonoBehaviour {

    [SerializeField] private Tower turretTower;
    [SerializeField] private Tower splashTower;

    [HideInInspector] public Tower choosenTower;
    private BuildingNode node;
    private PlayerManager playerManager;
    private UIManager uiManager;

    private void Awake() {
        GameManager.buildingManager = this;
    }

    private void Start() {
        playerManager = GameManager.playerManager;
        uiManager = playerManager.uiManager;
    }

    public void OnNodeClick(BuildingNode _node) {
        node = _node;
        uiManager.ActivateChooseTowerObject();
    }

    public void BuildTower() {
        if (choosenTower == null || node == null){
            return;
        }
        if (choosenTower.CostToBuild <= playerManager.Money){
            Build();
        } else{
            print("Not enough money");
        }
    }

    private void Build() {
        Instantiate(choosenTower, new Vector3(node.transform.position.x, node.transform.position.y + 0.5f, node.transform.position.z), Quaternion.identity, node.transform);
        GameManager.playerManager.AddMoney(-choosenTower.CostToBuild);
        node.Busy = true;
        node = null;
        choosenTower = null;
        uiManager.DiactivateChooseTowerObject();
    }
}