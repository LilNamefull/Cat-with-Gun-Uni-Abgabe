using UnityEngine;

public class EnemyWallMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component attached to this GameObject
    }

    private void FixedUpdate()
    {
        // Move the enemy to the left by setting its horizontal velocity
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);  // Keep the current vertical velocity (y-axis), but move horizontally to the left at 'moveSpeed'

        // Destroy the enemy when it goes off-screen (past the left side of the screen)
        if (transform.position.x < -17f)  // If the x position is less than -17, the enemy is off-screen
        {
            Destroy(gameObject);  // Destroy the enemy object
        }
    }
}
