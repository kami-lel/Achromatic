using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float WalkingSpeed = 0.1f;
    public GameObject circle;

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

        Vector3 pos = circle.transform.localPosition;
        float newPosX = Mathf.MoveTowards(pos.x, 0.0f, 5.0f * Time.fixedDeltaTime);
        pos.x = newPosX;
        circle.transform.localPosition = pos;
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
            Vector3 pos = circle.transform.localPosition;
            pos.x = 2.0f;
            circle.transform.localPosition = pos;

            rb.linearVelocityY += 2.0f;
        }
    }

    public void OnPowerJump()
    {
        if (IsOnFloor())
        {
            rb.linearVelocityY += 2.0f; // jump height
            Debug.Log("Player\tPower Jump");
        }
    }

    private bool IsOnFloor()
    {
        return rb.IsTouchingLayers(Physics2D.AllLayers);
    }

    // todo manually handle input timing
    //

    public void OnAction(InputAction.CallbackContext ctxt)
    {
        Debug.Log(ctxt); // HACK
    }
}
