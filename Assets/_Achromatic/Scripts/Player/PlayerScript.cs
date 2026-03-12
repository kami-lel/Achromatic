using System;
using Assets._Achromatic.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

// todo implements walking (vs running)

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript: MonoBehaviour {

    // public members  =========================================================

    [NonSerialized]
    public Rigidbody2D playerRB;

    // FIXME use multiple component approach
    // managers
    // [NonSerialized]
    // public Movement mvmt;

    [NonSerialized]
    public PlayerInputManager im;

    public event Action<String> OnTriggerEnter;
    public event Action<String> OnTriggerExit;

    // public methods  =========================================================

    public void EnsureFacingRight() {
        // mvmt.EnsureFacing(1);
    }


    // Inspector Fields  =======================================================

    public LayerMask groundLayerMask = Physics2D.AllLayers;

    public PhysicsMaterial2D defaultMaterial;

    public PhysicsMaterial2D noFrictionMaterial;

    // MonoBehavior Lifecycle  =================================================

    void Awake() {
        // mvmt = new(this);
        im = new(this);
    }

    private void Start() {
        // mvmt.Start();
        im.Start();

        GCS.I.states = GameState.EXPLORE;
    }

    void FixedUpdate() {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
            return;
        }

        // mvmt.FixedUpdate();
    }

    private void OnDisable() {
        im.OnDisable();
    }

    // Unity Messages  #########################################################

    private void OnTriggerEnter2D(Collider2D other) {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0 ||
                other == null || !other.isTrigger) {
            return;
        }

        Debug.Log("Player:\tenters trigger: " + other.tag);
        OnTriggerEnter?.Invoke(other.tag);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other == null || !other.isTrigger) {
            return;
        }

        OnTriggerExit?.Invoke(other.tag);
    }
}