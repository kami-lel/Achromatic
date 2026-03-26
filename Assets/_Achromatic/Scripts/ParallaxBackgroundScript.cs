// ParallaxBackgroundSingleLayer.cs
using UnityEngine;

public class ParallaxBackgroundSingleLayer: MonoBehaviour {
    // Todo improve, allow mux layers

    // Inspector Fields  #######################################################
    [SerializeField]
    private GameObject mainCamera;  // assign orthographic Camera in Inspector

    [SerializeField]
    private Sprite layerSprite;  // sprite used for single parallax layer

    [SerializeField]
    private float parallaxFactor = 0.2f;  // 0 far, 1 follow camera

    [SerializeField]
    private int tileCount = 3;  // number of side-by-side tiles, min 2

    [SerializeField]
    private Vector3 layerOffset = new Vector3(0f, 0f, 0f);  // local offset for layer

    [SerializeField]
    private bool followCameraY = false;  // allow vertical parallax if true

    // MonoBehaviour Lifecycle  #################################################
    private Vector3 previousCameraPosition;
    private Transform layerParent;
    private Transform[] tiles;
    private float tileWidth;

    void Start() {
        // validate inputs  -------------------------------------------------------
        if (mainCamera == null || layerSprite == null) {
            // Debug.LogError("Parallax:\tassign mainCamera and layerSprite");
            enabled = false;  // disable script on bad config
            return;
        }

        tileCount = Mathf.Max(2, tileCount);  // ensure at least 2 tiles
        previousCameraPosition = mainCamera.transform.position;
        initLayer();  // create tiles and parent
    }

    void Update() {
        // compute camera delta  ------------------------------------------------
        Vector3 camPos = mainCamera.transform.position;
        Vector3 delta = camPos - previousCameraPosition;
        previousCameraPosition = camPos;

        // move layer parent by scaled delta  ----------------------------------
        Vector3 move = new Vector3(delta.x * parallaxFactor,
                                   (followCameraY ? delta.y * parallaxFactor : 0f),
                                   0f);
        layerParent.position += move;

        // perform horizontal wrap relative to camera  --------------------------
        wrapTilesHorizontally(camPos.x);
    }



    // private members  ########################################################

    // initialize layer parent and tiles  ####################################
    private void initLayer() {
        // create parent holder  ------------------------------------------------
        GameObject parentGo = new GameObject("ParallaxLayer");
        parentGo.transform.parent = transform;
        parentGo.transform.localPosition = layerOffset;
        layerParent = parentGo.transform;

        // compute tile width in world units  ----------------------------------
        tileWidth = getSpriteWidth(layerSprite);
        if (tileWidth <= 0f) {
            Debug.LogError("Parallax:\tsprite has zero width");
            enabled = false;
            return;
        }

        // create tiles side-by-side  ------------------------------------------
        tiles = new Transform[tileCount];
        for (int i = 0; i < tileCount; i++) {
            GameObject tile = new GameObject("Tile" + i);
            tile.transform.parent = layerParent;
            tile.transform.localPosition = new Vector3(i * tileWidth, 0f, 0f);

            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = layerSprite;
            sr.sortingOrder = 0;  // adjust if needed

            tiles[i] = tile.transform;
        }
    }

    // compute sprite width in world units  ###################################
    private float getSpriteWidth(Sprite s) {
        if (s == null)
            return 0f;
        return s.bounds.size.x;
    }

    // wrap tiles horizontally around camera view  ############################
    private void wrapTilesHorizontally(float camX) {
        Camera cam = mainCamera.GetComponent<Camera>();
        if (cam == null || !cam.orthographic)
            return;  // require orthographic camera

        float halfViewWidth = cam.orthographicSize * cam.aspect;

        for (int i = 0; i < tiles.Length; i++) {
            Transform t = tiles[i];
            float tileLeft = t.position.x - (tileWidth * 0.5f);
            float tileRight = t.position.x + (tileWidth * 0.5f);

            // move tile right if completely left of view  ----------------------
            if (tileRight < camX - halfViewWidth) {
                float maxX = float.NegativeInfinity;
                for (int j = 0; j < tiles.Length; j++) {
                    if (tiles[j].position.x > maxX)
                        maxX = tiles[j].position.x;
                }
                t.position = new Vector3(maxX + tileWidth, t.position.y, t.position.z);
            }
            // move tile left if completely right of view  ----------------------
            else if (tileLeft > camX + halfViewWidth) {
                float minX = float.PositiveInfinity;
                for (int j = 0; j < tiles.Length; j++) {
                    if (tiles[j].position.x < minX)
                        minX = tiles[j].position.x;
                }
                t.position = new Vector3(minX - tileWidth, t.position.y, t.position.z);
            }
        }
    }
}