using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Members  #########################################################

    // Singleton
    public static SceneChanger I;

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        Debug.Log("LoadNextScene", this);

        // TODO set game state
        endingCamera.Priority = HIGH_CAMERA_PRIORITY;  // enable ending camera
        fadingBlockingPanel.FadeOut();
        // TODO load scene async
    }

    public void RegisterStaticCameras(
            CinemachineVirtualCamera startingCamera,
            CinemachineVirtualCamera endingCamera) {

        this.startingCamera = startingCamera;
        this.endingCamera = endingCamera;

        this.startingCamera.Priority = HIGH_CAMERA_PRIORITY;
        this.endingCamera.Priority = 0;
    }


    // Inspector Fields  #######################################################

    [SerializeField]
    private FadingBlockingPanel fadingBlockingPanel;

    // MonoBehavior Lifecycle  #################################################

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

    // Event Handler  ##########################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }


    // constants  ##############################################################

    private const int HIGH_CAMERA_PRIORITY = 100;

    // private members  ########################################################
    // cached references
    private CinemachineVirtualCamera startingCamera;
    private CinemachineVirtualCamera endingCamera;

    // private methods  ########################################################

    private void EnterNewScene() {
        Debug.Log("EnterNewScene", this);

        startingCamera.Priority = HIGH_CAMERA_PRIORITY;
        fadingBlockingPanel.FadeIn();

        // TODO clear camera priority

        GCS.I.states = GameState.EXPLORE;
    }

}

