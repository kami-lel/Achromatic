using UnityEngine;


public class ParallaxBackground: MonoBehaviour {

    // Inspector Fields  #######################################################
    [SerializeField]
    private float depth = 0.5f;
    [SerializeField]
    private GameObject left;
    [SerializeField]
    private GameObject right;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // test left & right  --------------------------------------------------
        if (!left.TryGetComponent<SpriteRenderer>(out leftSpriteRenderer)) {
            Debug.LogError("left must have SpriteRenderer Component", this);
        }

        if (!right.TryGetComponent<SpriteRenderer>(out rightSpriteRenderer)) {
            Debug.LogError("right must have SpriteRenderer Component", this);
        }
    }

    // private members  ########################################################
    // cached references
    private SpriteRenderer leftSpriteRenderer;
    private SpriteRenderer rightSpriteRenderer;
}