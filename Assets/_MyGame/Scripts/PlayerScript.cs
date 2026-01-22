using System;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float WalkingSpeed = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        float velocityX = rb.linearVelocityX;
        velocityX = Mathf.MoveTowards(velocityX, WalkingSpeed, 50.0f * Time.fixedDeltaTime);
        rb.linearVelocityX = velocityX;
    }

    public void OnJump()
    {
        if (IsOnFloor())
        {
            rb.linearVelocityY += 5.0f; // jump height
            Debug.Log("Player\tJump");
        }
    }

    public void OnDash()
    {
        if (IsOnFloor())
        {
            // TODO power dash
            rb.linearVelocityX += 10.0f;
            rb.linearVelocityY += 0.5f;
            Debug.Log("Player\tDash");
        }
    }

    public void OnPowerJump()
    {
        if (IsOnFloor())
        {
            rb.linearVelocityY += 8.0f; // jump height
            Debug.Log("Player\tPower Jump");
        }
    }

    private bool IsOnFloor()
    {
        return rb.IsTouchingLayers(Physics2D.AllLayers);
    }

    // todo manually handle input timing
}
