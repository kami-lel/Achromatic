using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerScript: MonoBehaviour {

    [SerializeField]
    private float WalkingSpeed = 0.1f;

    [SerializeField]
    private GameObject circle;

    [NonSerialized]
    public PlayerInputManager inputManager;

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

    private void FixedUpdate() {
        float velocityX = playerRB.linearVelocityX;
        velocityX = Mathf.MoveTowards(velocityX, WalkingSpeed, 50.0f * Time.fixedDeltaTime);
        playerRB.linearVelocityX = velocityX;

        Vector3 pos = circle.transform.localPosition;
        float newPosX = Mathf.MoveTowards(pos.x, 0.0f, 5.0f * Time.fixedDeltaTime);
        pos.x = newPosX;
        circle.transform.localPosition = pos;
    }

    public void OnJump() {
        if (IsOnFloor()) {
            playerRB.linearVelocityY += 5.0f; // jump height
            Debug.Log("Player\tJump");
        }
    }

    public void OnDash() {
        if (IsOnFloor()) {
            Vector3 pos = circle.transform.localPosition;
            pos.x = 2.0f;
            circle.transform.localPosition = pos;

            playerRB.linearVelocityY += 2.0f;
        }
    }

    public void OnPowerJump() {
        if (IsOnFloor()) {
            playerRB.linearVelocityY += 2.0f; // jump height
            Debug.Log("Player\tPower Jump");
        }
    }

    private bool IsOnFloor() {
        return playerRB.IsTouchingLayers(Physics2D.AllLayers);
    }
}
