
using System;
using System.Collections;

using UnityEngine;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;
using Unity.VisualScripting;


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


[RequireComponent(typeof(Transform))]
public class PieceScript: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private BeatmapMeta beatmapMeta;

    [Header("Triggers")]

    [SerializeField]
    private Collider2D startVampTrigger;

    [SerializeField]
    private Collider2D startPreludeTrigger;

    [Header("tmp")]

    [SerializeField]
    private PseudoAudioPlugin pseudoAudioPlugin;

    // MonoBehavior Lifecycle  #################################################
    void Awake() {
        music = new(pseudoAudioPlugin, beatmapMeta);
        prelude = new(music, startVampTrigger, startPreludeTrigger, GetComponent<Transform>().position);
    }

    private void Start() {
        notes = new(music, beatmapMeta);
        prefabs = new();

        scoreTracker = new(notes);
        criteria = new(music, notes, scoreTracker);

        playerManager = new();

        // inputs
        inputs = new(criteria, playerManager.playerInput);

        music.Start();
    }

    private void Update() {
        if (GameControllerScript.Instance.gameState == GameState.MAIN_PIECE) {
            criteria.Update();
            prefabs.Update();
            playerManager.Update();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("hi");  // HACK
        prelude.OnTriggerEnter2D(collision);
    }

    private void OnDisable() {
        playerManager.OnDisable();
        inputs.OnDisable(playerManager.playerInput);
    }

    // private members  ########################################################
    // managers
    private NotesManager notes;
    private PreludeManager prelude;
    private MusicManager music;
    private PrefabsManager prefabs;
    private Criteria criteria;
    private ScoreTracker scoreTracker;
    private PlayerManager playerManager;
    private InputManager inputs;
}


// Bug audio start is jarring, lose framerate
// Fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action