using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class PieceScript: MonoBehaviour {

    // Inspector Fields  -------------------------------------------------------
    // PieceScript will take over control of player during this piece
    [SerializeField]
    private GameObject player;

    // todo move some data to beatmap file
    // factor for player horizontal speed
    [SerializeField]
    private float beatSpeed = 1.0f;

    [Header("Music Settings")]
    // piece chart file
    [SerializeField]
    private TextAsset beatmapFile;

    // time signature of the music pice
    [SerializeField]
    private int beatPerBar = 4;

    // tempo i.e. bpm of the music
    [SerializeField]
    private float tempo = 120.0f;

    // length of music before the actual play, in second
    [SerializeField]
    private float preludeLength = 0.0f;

    // cls properties  ---------------------------------------------------------
    private AudioSource audioSource;
    private Rigidbody2D playerRB;
    private Vector2 origin;
    private PieceBeatmap beatmap;

    private float _tempoDiv60;
    private float _preludeOffset;

    /// <summary>
    /// initialize PieceScript
    /// </summary>
    public void Awake() {
        playerRB = player.GetComponent<Rigidbody2D>();
        origin = (Vector2) transform.position;

        beatmap = new PieceBeatmap(beatmapFile);

        _tempoDiv60 = tempo / 60.0f;
        _preludeOffset = preludeLength * _tempoDiv60;

        // set up audio source  ------------------------------------------------
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// start this music piece
    /// </summary>
    public void OnEnable() {
        // move player to Piece's Transform's position
        player.GetComponent<PlayerScript>().controlledByPiece = true;

        playerRB.MovePosition(origin);

        // start the music
        audioSource.Play();
    }


    public void Update() {
        // update user horizontal position
        float x = transform.position.x + CalcCurrentBeatCount() * beatSpeed;
        Vector2 newPosition = new(x, playerRB.position.y);
        playerRB.MovePosition(newPosition);
    }

    private void OnDisable() {
        player.GetComponent<PlayerScript>().controlledByPiece = false;
    }

    private float CalcCurrentBeatCount() {
        return audioSource.time * _tempoDiv60 - _preludeOffset;
    }
}
