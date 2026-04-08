using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Members  #########################################################

    // Singleton
    public static SceneChanger I;

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        // enable ending camera
        endingCamera.Priority = 10;
        fadingBlockingPanel.FadeOut();
        // TODO set game state

        // TODO scene changer load next scene
        Debug.LogError("scene changer load next scene: " + sceneName, this);
    }

    public void RegisterStaticCameras(
            CinemachineVirtualCamera startingCamera,
            CinemachineVirtualCamera endingCamera) {

        this.startingCamera = startingCamera;
        this.endingCamera = endingCamera;

        this.startingCamera.Priority = 10;
        this.endingCamera.Priority = 0;
    }


    // Inspector Fields  #######################################################

    [SerializeField]
    private FadingBlockingPanel fadingBlockingPanel;

    [SerializeField]
    private float fadeDuration = 1f;

    // MonoBehavior Lifecycle  ###################################################

    private void Awake() {
        if (fadingBlockingPanel == null) {
            Debug.LogError("must assign: fadingBlockingPanel", this);
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Event Handler  #############################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }


    // private members  ########################################################
    // cached references
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

