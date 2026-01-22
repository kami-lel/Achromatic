using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // TODO
        Vector2 velocity = rb.linearVelocity;
        velocity.x = 5.0f;
        rb.linearVelocity = velocity;
    }
}
