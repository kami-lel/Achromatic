using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        // TODO load next scene
        Debug.LogError("scene changer load next scene: " + sceneName);
    }

    // Event Handler  ##########################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // private methods  ########################################################

    private void EnterNewScene() {
        // TODO enter new scene
        Debug.LogError("new scene logic");
    }

}
