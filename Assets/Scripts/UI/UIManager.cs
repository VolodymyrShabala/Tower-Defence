using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Text moneyText;
    [SerializeField] private Text livesText;

    [SerializeField] private RectTransform chooseTowerGameObject;
    private PlayerManager playerManager;
    private BuildingManager buildingManager;

    private void Awake() {
        GameManager.playerManager.uiManager = this;
    }

    private void Start() {
        buildingManager = GameManager.buildingManager;
        playerManager = GameManager.playerManager;

        livesText.text = "Lives: " + playerManager.Lives;
        moneyText.text = "Money: " + playerManager.Money;
        
        chooseTowerGameObject.gameObject.SetActive(false);
    }

    public void UpdateMoneyText() {
        moneyText.text = "Money: " + playerManager.Money;
    }

    public void UpdateLivesText() {
        livesText.text = "Lives: " + playerManager.Lives;
    }

    public void ChooseTower(Tower t) {
        //can have sprite with tower image following the mouse
        GameManager.buildingManager.choosenTower = t;
        buildingManager.BuildTower();
    }

    public void ActivateChooseTowerObject() {
        chooseTowerGameObject.position = Input.mousePosition;
        chooseTowerGameObject.gameObject.SetActive(true);
    }

    public void DiactivateChooseTowerObject() {
        chooseTowerGameObject.gameObject.SetActive(false);
    }
}