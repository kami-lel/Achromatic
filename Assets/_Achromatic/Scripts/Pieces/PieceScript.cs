
using System;

using UnityEngine;
using UnityEngine.Splines;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;
using Assets._Achromatic.Scripts.Beatmap;
using Cinemachine;


// Bug audio start is jarring, lose framerate


/// <summary>
/// controller during <c>Music Play</c>, enables:
/// <list type="bullet">
///   <item><description>
///     load and parse beatmap <c>.json</c> file
///   </description></item>
///   <item><description>
///     dynamically create and place <c>BeatmapElements</c> prefabs in scene
///   </description></item>
///   <item><description>
///   control player movement during play
///   </description></item>
/// </list>
/// </summary>
[RequireComponent(typeof(Transform))]
public class PieceScript: MonoBehaviour {

    // public members  #########################################################

    [NonSerialized]
    public Vector2 preludeStartOrigin;

    // Fixme use multiple component approach
    // managers
    public Starter starter;
    public MusicManager music;

    public Criteria criteria;
    public Score score;
    public PlayerManager playerManager;
    public InputManager inputs;
    public Beatmap beatmap;

    public ElementsManager elements;

    // Inspector Fields  #######################################################

    [SerializeField]
    private BeatmapMeta beatmapMeta;

    public int debugMusicStaringBar = 0;

    [Header("Internals")]
    public AnimationCurve vampDistantVsVolume =
            AnimationCurve.Linear(0, 1, 30, 0);

    [SerializeField]
    private Transform startPreludeTransform;

    public SplineContainer mainPartPath;

    public CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Transform prefabs;

    [SerializeField]
    private AnimationCurve jumpHeightVsTime;

    [SerializeField]
    private AnimationCurve attackOffsetVsTime;

    [SerializeField]
    private AnimationCurve squatOffsetVsTime;

    [Header("tmp")]

    [SerializeField]
    private PseudoAudioPlugin pseudoAudioPlugin;

    // Hack
    public Transform playerSprite;


    // MonoBehavior Lifecycle  #################################################

    private void Start() {
        if (mainPartPath == null) {
            Debug.LogError("Piece:\tmust assign mainPartPath");
        }

        preludeStartOrigin = startPreludeTransform.position;


        beatmap = new(this, beatmapMeta);
        music = new(this, pseudoAudioPlugin);
        score = new(beatmap);
        criteria = new(this);
        playerManager = new(this, jumpHeightVsTime, attackOffsetVsTime, squatOffsetVsTime);
        starter = new(this);
        inputs = new(this);

        elements = new(this, prefabs);

        music.Start();

        virtualCamera.Priority = 0;
    }

    private void Update() {
        starter.Update();
        playerManager.Update();
        beatmap.Update();
        elements.Update();
        criteria.Update();
    }

    private void OnDisable() {
        inputs.OnDisable();
        starter.OnDisable();
    }

    private void FixedUpdate() {
        playerManager.FixedUpdate();
    }

}
