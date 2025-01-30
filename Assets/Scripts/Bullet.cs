using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector2 direction = new Vector2(1, 0);
    public float speed = 2;
    public int damage = 1; 
    public bool isEnemy = false; 

    public Vector2 velocity;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destroy the bullet after 1.2 seconds to avoid it staying in the game forever
        Destroy(gameObject, 1.2f);

        // Ensures that the bullet does not get destroyed when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the velocity by multiplying direction by speed
        velocity = direction * speed;
    }

    // FixedUpdate is called once per physics frame, and is used to move the bullet
    private void FixedUpdate()
    {
        // Get the current position of the bullet
        Vector2 pos = transform.position;

        // Update the position by adding velocity
        pos += velocity * Time.fixedDeltaTime;

        // Apply the updated position to the bullet's transform
        transform.position = pos;
    }

    // Trigger collision handling
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet is an enemy bullet (i.e., comes from an enemy)
        if (isEnemy)
        {
            // If the bullet hits the player, deal damage
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage); // Player takes damage
                Destroy(gameObject); // Destroy the bullet after impact
            }

            // Ignore collisions with certain objects that shouldn't interact with enemy bullets
            if (collision.CompareTag("Boss") || collision.CompareTag("BossBullet") || collision.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
                return;
            }
        }
        else
        {
            // If the bullet is from the player and it hits the boss, deal damage
            if (collision.CompareTag("Boss"))
            {
                BossScript boss = collision.GetComponent<BossScript>();
                if (boss != null)
                {
                    boss.TakeDamage(damage); // Boss takes damage
                    Destroy(gameObject); // Destroy the bullet after impact
                }
                return;
            }
        }

        // If the bullet hits anything else that is not the player, destroy it
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

