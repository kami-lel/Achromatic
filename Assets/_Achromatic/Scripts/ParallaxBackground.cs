using UnityEngine;
using Cinemachine;

public class ParallaxBackground: MonoBehaviour {
    // Inspector Fields  #######################################################
    [SerializeField]
    private float parallaxFactor = 0.5f;
    [SerializeField]
    private GameObject left;
    [SerializeField]
    private GameObject right;

    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
        // deal with left & right  ---------------------------------------------
        if (left == null) {
            Debug.LogError("must assign left", this);
            return;
        }
        if (right == null) {
            Debug.LogError("must assign right", this);
            return;
        }
        left.SetActive(true);
        right.SetActive(true);
        if (left.GetComponent<SpriteRenderer>() == null) {
            Debug.LogError("left must have SpriteRenderer Component", this);
        }
        if (right.GetComponent<SpriteRenderer>() == null) {
            Debug.LogError("right must have SpriteRenderer Component", this);
        }
        if (left.TryGetComponent<SpriteRenderer>(out var sr)) {
            tileWidth = sr.bounds.size.x;
        }
        // init right position
        right.transform.position = new Vector3(
            left.transform.position.x + tileWidth,
            right.transform.position.y,
            right.transform.position.z);
    }

    private void OnEnable() {
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable() {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    // Cinemachine Update  #####################################################

    private void OnCameraUpdated(CinemachineBrain brain) {
        Vector3 delta = cam.position - lastCamPos;
        Vector3 parallaxDelta = new Vector3(
            delta.x * parallaxFactor,
            delta.y * parallaxFactor,
            0);
        left.transform.position += parallaxDelta;
        right.transform.position += parallaxDelta;
        lastCamPos = cam.position;

        // tiling logic  -------------------------------------------------------
        GameObject actualLeft, actualRight;
        if (left.transform.position.x <= right.transform.position.x) {
            actualLeft = left;
            actualRight = right;
        } else {
            actualLeft = right;
            actualRight = left;
        }

        float camX = cam.position.x;

        if (camX > actualRight.transform.position.x) {
            actualLeft.transform.position = new Vector3(
                actualRight.transform.position.x + tileWidth,
                actualLeft.transform.position.y,
                actualLeft.transform.position.z);
        } else if (camX < actualLeft.transform.position.x) {
            actualRight.transform.position = new Vector3(
                actualLeft.transform.position.x - tileWidth,
                actualRight.transform.position.y,
                actualRight.transform.position.z);
        }
    }

    // private members  ########################################################
    private float tileWidth;
    private Vector3 lastCamPos;
    private Transform cam;
}