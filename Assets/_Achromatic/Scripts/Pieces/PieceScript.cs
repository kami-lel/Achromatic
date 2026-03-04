

using UnityEngine;
using UnityEngine.Splines;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;
using Assets._Achromatic.Scripts.Beatmap;



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

    // Inspector Fields  #######################################################

    [SerializeField]
    private BeatmapMeta beatmapMeta;


    public int debugMusicStaringBar = 0;

    [Header("Internals")]
    public AnimationCurve vampDistantVsVolume = AnimationCurve.Linear(0, 1, 30, 0);

    [SerializeField]
    private Transform vampLoudestOrigin;

    public SplineContainer mainPartPath;

    [Header("tmp")]

    [SerializeField]
    private PseudoAudioPlugin pseudoAudioPlugin;

    // MonoBehavior Lifecycle  #################################################

    private void Start() {
        if (mainPartPath == null) {
            Debug.LogError("must assign mainPartPath");
        }

        beatmap = new(this, beatmapMeta);
        music = new(pseudoAudioPlugin);
        notes = new(music, beatmapMeta);
        prefabs = new();
        scoreTracker = new(notes);
        criteria = new(music, notes, scoreTracker);
        playerManager = new(this);
        starter = new(this, vampLoudestOrigin);
        inputs = new(criteria, playerManager.playerInput);

        music.Start();
    }

    private void Update() {
        starter.Update();

        if (isControllingPlayer) {
            beatmap.Update();
            playerManager.Update();
            criteria.Update();
        }
    }

    private void OnDisable() {
        starter.OnDisable();
        inputs.OnDisable(playerManager.playerInput);
    }

    // public members  #########################################################
    public bool isControllingPlayer = false;

    // managers
    public NotesManager notes;
    public Starter starter;
    public MusicManager music;
    public PrefabsPool prefabs;
    public Criteria criteria;
    public ScoreTracker scoreTracker;
    public PlayerManager playerManager;
    public InputManager inputs;
    public Beatmap beatmap;
}


// Bug audio start is jarring, lose framerate
// Fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action