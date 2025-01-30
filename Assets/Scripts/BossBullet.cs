using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage = 1;  

    void Start()
    {
        // Destroy the bullet after 5 seconds
        Destroy(gameObject, 5);

        // Prevent the bullet from being destroyed when switching scenes
        DontDestroyOnLoad(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // If the bullet collides with an enemy, ignore the collision (it won't affect enemies)
        if (collision.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);  // Ignore collision with enemies
            return;  // Skip further code in this method
        }

        // If the bullet collides with the player
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();  // Get the PlayerMovement component from the player object

            if (player != null)
            {
                player.TakeDamage(damage);  // Apply damage to the player
            }

            Destroy(gameObject);  // Destroy the bullet after it hits the player
        }

        // If the bullet collides with anything that's not the boss, destroy the bullet
        if (!collision.CompareTag("Boss"))
        {
            Destroy(gameObject);  // Destroy the bullet when it collides with any object that isn't the boss
        }
    }
}
