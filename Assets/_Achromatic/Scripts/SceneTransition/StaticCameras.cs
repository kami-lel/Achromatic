using Cinemachine;
using UnityEngine;

public class StaticCameras: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField] private CinemachineVirtualCamera startingCamera;

    [SerializeField] private CinemachineVirtualCamera endingCamera;


    // MonoBehavior Lifecycle  #################################################

    private void Start() {
        // check assigned
        if (startingCamera == null) {
            Debug.LogError("must assign: startingCamera", this);
            return;
        }
        if (endingCamera == null) {
            Debug.LogError("must assign: endingCamera", this);
            return;
        }

        // register
        SceneChanger.I.RegisterStaticCameras(startingCamera, endingCamera);

    }

}
