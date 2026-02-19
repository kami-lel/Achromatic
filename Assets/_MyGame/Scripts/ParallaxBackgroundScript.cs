using UnityEngine;

public class ParallaxBackgroundScript: MonoBehaviour {

    // TODO implement parallax bg

    // Inspector Fields  #######################################################

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private float parallaxEffectAmount;

    [Header("Background Sprites")]

    [SerializeField]
    private Sprite sprite1;

    [SerializeField]
    private Sprite sprite2;

    [SerializeField]
    private Sprite sprite3;

    // MonoBehavior Lifecycle  #################################################

    void Start() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffectAmount);
        float dist = (mainCamera.transform.position.x * parallaxEffectAmount);

        transform.position = new Vector3(
            startpos + dist,
            transform.position.y,
            transform.position.z
        );

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }



    // private members  ########################################################


    private float length,
        startpos;

}
