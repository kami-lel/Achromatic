using UnityEngine;

// bug prelude not functioning, currently only working w/ prelude = 0

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class PieceScript: MonoBehaviour {

    // Inspector Fields  #######################################################
    // PieceScript will take over control of player during this piece
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private TextAsset beatmapFile;


    // private members  ########################################################
    private AudioSource audioSource;
    private Rigidbody2D playerRB;
    private Vector2 origin;
    private PieceBeatmap beatmap;
    private PlayerScript playerScript;

    private float tempoDiv60;
    private float preludeOffsetAsBeat;


    // MonoBehavior Lifecycle  #################################################

    /// <summary>
    /// initialize PieceScript
    /// </summary>
    public void Awake() {
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerScript>();
        origin = (Vector2) transform.position;

        // set up audio source  ------------------------------------------------
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // set up beatmap  -----------------------------------------------------
        if (beatmapFile == null) {
            Debug.LogWarning("PieceScript: must provide beatmapFile");
        }
        beatmap = JsonUtility.FromJson<PieceBeatmap>(beatmapFile.text);

        tempoDiv60 = beatmap.tempo / 60.0f;
        preludeOffsetAsBeat = beatmap.preludeLength * tempoDiv60;
    }

    /// <summary>
    /// start this music piece
    /// </summary>
    public void OnEnable() {
        playerScript.SetPlayTypeAsExplore(false);

        // move player to Piece's Transform's position
        playerRB.MovePosition(origin);

        // start the music
        audioSource.Play();
    }

    public void Update() {
        // update user horizontal position
        float x = transform.position.x
                + CalcCurrentBeatCount() * beatmap.beatSpeed;
        Vector2 newPosition = new(x, playerRB.position.y);
        playerRB.MovePosition(newPosition);
    }

    private void OnDisable() {
        playerScript.SetPlayTypeAsExplore(true);
    }

    // helper methods  #########################################################

    private float CalcCurrentBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }
}
