using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // TODO
        Vector2 movement = new Vector2(1.0f, 1.0f);
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
}
