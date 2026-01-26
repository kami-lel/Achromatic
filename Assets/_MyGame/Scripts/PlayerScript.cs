using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerScript: MonoBehaviour {

    // todo implement RGB controls

    // Inspector Fields  -------------------------------------------------------
    [SerializeField]
    private float WalkingSpeed = 0.1f;

    [SerializeField]
    private GameObject circle;

    // properties  -------------------------------------------------------------
    [NonSerialized]
    public PlayerInputManager inputManager;

    // whether controlled by PieceScript
    public bool controlledByPiece = false;

    private Rigidbody2D playerRB;

    public void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        playerRB.gravityScale = 1.0f;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnEnable() {
        inputManager = new(GetComponent<PlayerInput>());
    }

    public void OnDisable() {
        inputManager?.Dispose();
        inputManager = null;
    }

    private bool IsOnFloor() {
        return playerRB.IsTouchingLayers(Physics2D.AllLayers);
    }
}
