using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private GameObject gameTitel;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject chooseLevelButtons;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    private bool sceneIsLoading;

    private void Start() {
        mainMenuButtons.SetActive(true);
        chooseLevelButtons.SetActive(false);
        loadingScreen.SetActive(false);
    }

    private void Update() {
        if (sceneIsLoading){
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!mainMenuButtons.activeInHierarchy){
                Return();
            }
        }
    }

    public void Return() {
        mainMenuButtons.SetActive(true);
        chooseLevelButtons.SetActive(false);
    }

    public void LoadLevel(int index) {
        gameTitel.SetActive(false);
        chooseLevelButtons.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadingScreen(index));
    }

    private IEnumerator LoadingScreen(int index) {
        sceneIsLoading = true;
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;

        while (!async.isDone){
            slider.value = async.progress;
            if (async.progress >= 0.9f){
                slider.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void StartGame() {
        mainMenuButtons.SetActive(false);
        chooseLevelButtons.SetActive(true);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }
}