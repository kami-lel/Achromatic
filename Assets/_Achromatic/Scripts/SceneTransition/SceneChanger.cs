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

        CinemachineVirtualCamera ending =
                FindVirtualCameraByTag(END_CAMERA_TAG);
        ending.Priority = HIGH_CAMERA_PRIORITY;
        fadingBlockingPanel.FadeOut();

        // TODO load scene async
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
    private const int LOW_CAMERA_PRIORITY = 0;
    private const string START_CAMERA_TAG = "startingCamera";
    private const string END_CAMERA_TAG = "endingCamera";

    // private methods  ########################################################

    private void EnterNewScene() {
        Debug.Log("EnterNewScene", this);

        CinemachineVirtualCamera startingCamera =
                FindVirtualCameraByTag(START_CAMERA_TAG);
        startingCamera.Priority = HIGH_CAMERA_PRIORITY;

        fadingBlockingPanel.FadeIn(onComplete: () => {
            startingCamera.Priority = LOW_CAMERA_PRIORITY;
        });

        GCS.I.states = GameState.EXPLORE;
    }


    // helpers  ================================================================

    private CinemachineVirtualCamera FindVirtualCameraByTag(string tag) {
        GameObject go;
        go = GameObject.FindGameObjectWithTag(tag);
        if (go == null) {
            Debug.LogError(
                $"fail to find Virtual Camera with tag: ${tag}"
            );
            return null;
        }

        var virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null) {
            Debug.LogError(
                $"GameObject does not have Virtual Camera Component"
            );
        }

        return virtualCamera;
    }

}

