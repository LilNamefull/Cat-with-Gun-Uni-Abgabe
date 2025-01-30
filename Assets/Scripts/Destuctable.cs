using UnityEngine;

public class Destuctable : MonoBehaviour
{
    bool canBeDestroyed = false;
    public int scoreValue = 100;
    public int damage = 1; 
    public int maxHealth = 30;
    public int currentHealth;

    AudioManager audioManager;

    private void Awake()
    {
        // Find and assign the AudioManager instance to play sounds
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the initial health to the maximum health
        currentHealth = maxHealth;

        // Add this enemy to the destructible count in the Level Manager
        Level.instance.AddDestuctable();
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy's X position is less than 8 and it can now be destroyed
        if (transform.position.x < 8f && !canBeDestroyed)
        {
            // Allow the enemy to be destroyed
            canBeDestroyed = true;

            // Get all gun components from this enemy's children
            Gun[] guns = transform.GetComponentsInChildren<Gun>();

            // Activate all guns (enabling them to shoot)
            foreach (Gun gun in guns)
            {
                gun.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player collision check
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && player.isInvincible)
        {
            // If the player is invincible, the enemy dies
            Die();
            return;
        }

        // Bullet collision check
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            // If the bullet is from an enemy
            if (bullet.isEnemy)
            {
                Debug.Log("Gegner Kugel hat getroffen!");
                player.TakeDamage(bullet.damage); // Deal damage to the player
                Destroy(bullet.gameObject); // Destroy the enemy bullet
            }
            else // If the bullet is from the player
            {
                TakeDamage(bullet.damage); // Deal damage to the enemy
                Destroy(bullet.gameObject); // Destroy the player's bullet
            }
        }
    }

    // OnDestroy is called when the object is about to be destroyed
    private void OnDestroy()
    {
        // Remove this enemy from the destructible count in the Level Manager
        Level.instance.RemoveDestuctable();
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        // Decrease current health by the damage amount
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} hat {damage} schaden genommen. Übrige Leben: {currentHealth}");

        // If health drops to 0 or below, the enemy dies
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method called when the enemy dies
    private void Die()
    {
        // Log the enemy's destruction
        Debug.Log($"{gameObject.name} wurde zerstört!");

        // Optionally add points to the player
        Level.instance.AddScore(scoreValue);

        // Destroy the enemy GameObject
        Destroy(gameObject);

        // Play death sound effect
        audioManager.PlaySFX(audioManager.DeathEnemy);
    }

}
