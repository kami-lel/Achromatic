using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Members  #########################################################

    // Singleton
    public static SceneChanger I;

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        // TODO scene changer load next scene
        Debug.LogError("scene changer load next scene: " + sceneName, this);
    }

    public void RegisterStaticCameras(
            CinemachineVirtualCamera startingCamera,
            CinemachineVirtualCamera endingCamera) {

        this.startingCamera = startingCamera;
        this.endingCamera = endingCamera;

        this.startingCamera.Priority = 10;
    }

    // Event Handler  #############################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }

    // MonoBehavior Lifecycle  ###################################################

    private void Awake() {
        I = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // private members  ########################################################
    private CinemachineVirtualCamera startingCamera;
    private CinemachineVirtualCamera endingCamera;

    // private methods  ########################################################

    private void EnterNewScene() {
        /* TODO scene changer enter new scene
        return;
        GCS.I.states = GameState.SCENE_TRANSITION;

        Debug.LogError("new scene logic");
        */
    }

}

