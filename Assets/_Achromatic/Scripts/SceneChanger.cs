using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Event Handler  ##########################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }

    // MonoBehavior Lifecycle  #################################################

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;

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
