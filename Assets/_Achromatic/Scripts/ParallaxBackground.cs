using UnityEngine;


public class ParallaxBackground: MonoBehaviour {

    // Inspector Fields  #######################################################
    [SerializeField]
    private float parallaxFactor = 0.5f;
    [SerializeField]
    private GameObject left;
    [SerializeField]
    private GameObject right;

    // MonoBehavior Lifecycle  #################################################

    // TODO make connected

    private void Awake() {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        // test left & right  --------------------------------------------------
        if (!left.TryGetComponent<SpriteRenderer>(out leftSpriteRenderer)) {
            Debug.LogError("left must have SpriteRenderer Component", this);
        }

        if (!right.TryGetComponent<SpriteRenderer>(out rightSpriteRenderer)) {
            Debug.LogError("right must have SpriteRenderer Component", this);
        }
    }

    private void LateUpdate() {
        Vector3 delta = cam.position - lastCamPos;
        left.transform.position += new Vector3(
                delta.x * parallaxFactor,
                delta.y * parallaxFactor,
                0);

        lastCamPos = cam.position;
    }


    // constants  ##############################################################
    private const float BOUND_SIZE = 20f;

    // private members  ########################################################

    private Vector3 lastCamPos;

    // cached references
    private SpriteRenderer leftSpriteRenderer;
    private SpriteRenderer rightSpriteRenderer;
    private Transform cam;

}

