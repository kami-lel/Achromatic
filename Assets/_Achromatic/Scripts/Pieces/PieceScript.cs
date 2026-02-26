
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;


// todo allows & give feedback for smashing input during: empty or climax
// todo background music during explore play
// Bug piece will have error if Active at beginning of scene


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
[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class PieceScript: MonoBehaviour {

    // Inspector Fields  #######################################################
    [SerializeField]
    private TextAsset beatmapFile;

    /// <summary>
    /// start this piece of music at bar <i>n</i>,
    /// default to <c>0.0f</c> for normal play
    /// </summary>
    [SerializeField]
    private float debugMusicStaringBar = 0.0f;

    [Header("Triggers")]

    [SerializeField]
    private Collider2D playerCollider;

    [SerializeField]
    private Collider2D playStartHitBox;



    // MonoBehavior Lifecycle  #################################################
    private void Start() {
        phase = Phase.INIT;

        music = new(GetComponent<AudioSource>());
        notes = new(beatmapFile, music);

        criteria = new(notes.data, scoreTracker);
        scoreTracker = new(notes.data);

        inputs = new();
    }

    private void Update() {
        // Todo better phase management
        float dist = Vector2.Distance(playerCollider.transform.position, playStartHitBox.transform.position);
        switch (phase) {
        case Phase.INIT:
            if (dist <= 20.0f) {
                Debug.Log("Start Prelude");
                phase = Phase.PRELUDE;
            }
            break;

        case Phase.PRELUDE:
            if (playStartHitBox.IsTouching(playerCollider)) {
                playerManager.StartControl();
            } else if (dist > 20.0f) {
                phase = Phase.INIT;
            }
            break;

        default:
            break;
        }

        // update managers  ----------------------------------------------------
        music.Update(phase, dist);


        if (phase == Phase.MAIN_PLAY) {
            criteria.Update(music.Time);
            prefabs.Update();
            playerManager.Update();
        }
    }

    void OnDisable() {
        playerManager.OnDisable();
    }

    // private members  ########################################################
    private Phase phase = Phase.INIT;

    // managers
    private Music music;
    private Notes notes;
    private PlayerManager playerManager;
    private InputManager inputs;
    private PrefabsManager prefabs;


    // scores
    private Criteria criteria;
    private ScoreTracker scoreTracker;
}


// Bug audio start is jarring, lose framerate
// Fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action