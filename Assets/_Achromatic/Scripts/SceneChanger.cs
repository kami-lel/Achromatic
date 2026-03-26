using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Event Handler  ##########################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // special case for 1st ever scene, when game start
        if (isFirstScene) {
            EnterNewScene();
            isFirstScene = false;
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // private members  ########################################################
    private bool isFirstScene = true;


    // private methods  ########################################################

    private void EnterNewScene() {
        Debug.LogError("new scene logic");  // HACK
    }

}
