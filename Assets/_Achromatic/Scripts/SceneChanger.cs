using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        // todo scene changer load next scene
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
        return;  // hack
        GCS.I.states = GameState.SCENE_TRANSITION;

        // todo scene changer enter new scene
        Debug.LogError("new scene logic");
    }

}
