using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private int lives = 100;
    [SerializeField] private int money = 100;

    [HideInInspector] public UIManager uiManager;

    private void Awake() {
        GameManager.playerManager = this;
    }

    public void AddMoney(int _money) {
        money += _money;
        uiManager.UpdateMoneyText();
    }

    public void TakeDamage(int damage) {
        lives -= damage;
        if (lives <= 0){
            GameOver();
        }
        uiManager.UpdateLivesText();
    }

    private void GameOver() {
        Debug.Log("You've lost this one!");
    }

    public int Money {
        get {
            return money;
        }
    }

    public int Lives {
        get {
            return lives;
        }
    }
}