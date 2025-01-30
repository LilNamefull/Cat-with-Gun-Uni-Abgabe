using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Gun[] guns;              // Array of guns attached to the player
    public float moveSpeed;         // Player movement speed
    private float speedX, speedY;   // Player movement on X and Y axes
    Rigidbody2D rb;                 // Rigidbody2D component for physics-based movement
    private float minX = -9f, maxX = 0f, minY = -4.7f, maxY = 1.5f;  // Movement boundaries
    public int lives = 3;           // Player's lives
    public bool isInvincible = false;  // Flag for invincibility

    bool shoot;                     // Flag for shooting
    SpriteRenderer spriteRenderer;  // Renderer for the player sprite

    GameObject shield;              // Shield object
    int powerUpGunLevel = 0;        // Power-up level for guns
    public float gunPowerUpDuration = 4f;  // Duration for the gun power-up

    AudioManager audioManager;      // AudioManager for playing sound effects
    public GameManager gameManager; // GameManager to handle game over
    private bool isDead;            // Flag for player death

    public GameObject explosionPrefab;  // Explosion effect prefab

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();  // Find AudioManager
    }

    void Start()
    {
        shield = transform.Find("Shield").gameObject;  // Find the shield object in the player
        DeactivateShield();                            // Initially deactivate the shield

        guns = transform.GetComponentsInChildren<Gun>();  // Get all Gun components attached to the player
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.powerUpLevelRequirement != 0)
            {
                gun.gameObject.SetActive(false);  // Deactivate guns that require a specific power-up level
            }
        }

        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component for movement
    }

    void Update()
    {
        // Player movement using arrow keys or WASD
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(speedX, speedY);
    
        // Position clamping (keep player within the defined boundaries)
        Vector3 clampedPosition = transform.position;

        // X-axis clamping
        if (clampedPosition.x < minX)
        {
            clampedPosition.x = minX;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else if (clampedPosition.x > maxX)
        {
            clampedPosition.x = maxX;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Y-axis clamping
        if (clampedPosition.y < minY)
        {
            clampedPosition.y = minY;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        else if (clampedPosition.y > maxY)
        {
            clampedPosition.y = maxY;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        // Apply the clamped position
        transform.position = clampedPosition;

        // Handle shooting input
        shoot = Input.GetKeyDown(KeyCode.Space);
        if (shoot)
        {
            shoot = false;
            foreach (Gun gun in guns)
            {
                if (gun.gameObject.activeSelf)
                {
                    gun.Shoot();  // Trigger shooting from active guns
                    audioManager.PlaySFX(audioManager.ShootPlayer);  // Play shooting sound effect
                }
            }
        }
    }

    void ActivateShield()
    {
        shield.GetComponent<Shield>().ActivateShield(10f);  // Activate the shield for 10 seconds
    }

    void DeactivateShield()
    {
        shield.GetComponent<Shield>().DeactivateShield();  // Deactivate the shield
    }

    bool HasShield()
    {
        return shield.activeSelf;  // Check if the shield is active
    }

    void AddGuns()
    {
        powerUpGunLevel++;  // Increment the gun power-up level
        foreach (Gun gun in guns)
        {
            if (gun.powerUpLevelRequirement == powerUpGunLevel)
            {
                gun.gameObject.SetActive(true);  // Activate the gun matching the power-up level
            }
        }
        StartCoroutine(DeactivateGunsAfterDuration());  // Deactivate guns after power-up duration
    }

    private IEnumerator DeactivateGunsAfterDuration()
    {
        yield return new WaitForSeconds(gunPowerUpDuration);  // Wait for the power-up duration

        // Deactivate guns after duration
        foreach (Gun gun in guns)
        {
            if (gun.powerUpLevelRequirement == powerUpGunLevel)
            {
                gun.gameObject.SetActive(false);
            }
        }

        powerUpGunLevel--;  // Reduce the power-up gun level
    }

    void Invincible()
    {
        StartCoroutine(InvincibilityRoutine());  // Start invincibility routine
    }

    public IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float blinkDuration = 2f;  // Duration of invincibility
        float blinkInterval = 0.2f;  // Interval between blinks

        float elapsed = 0f;
        while (elapsed < blinkDuration)
        {
            sr.enabled = !sr.enabled;  // Toggle sprite visibility
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sr.enabled = true;  // Ensure the player is visible at the end
        isInvincible = false;  // Reset invincibility
    }

    // Handle damage
    public void TakeDamage(int damage)
    {
        if (HasShield())
        {
            DeactivateShield();  // Deactivate shield if active
        }
        else if (!isInvincible)
        {
            lives -= damage;  // Decrease lives based on damage
            Debug.Log($"Player took {damage} damage. Remaining lives: {lives}");

            if (lives <= 0 && !isDead)
            {
                isDead = true;
                gameManager.gameOver();  // Game over if no lives remain
            }
            else
            {
                StartCoroutine(InvincibilityRoutine());  // Activate invincibility after taking damage
            }
        }
    }

    public void ResetPlayer()
    {
        lives = 3;  // Reset lives
        isInvincible = false;  // Reset invincibility
        isDead = false;  // Reset death flag
        Debug.Log("Player stats reset.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with different objects

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null && bullet.isEnemy && !isInvincible)
        {
            Debug.Log("Hit by enemy bullet!");
            ShowExplosion();
            TakeDamage(bullet.damage);  // Take damage from enemy bullet
            Destroy(bullet.gameObject);  // Destroy the bullet
        }
        else if (collision.GetComponent<Destuctable>() != null)
        {
            Debug.Log("Hit by an enemy!");
            Destuctable destuctable = collision.GetComponent<Destuctable>();

            if (isInvincible)
            {
                Destroy(destuctable.gameObject);  // Destroy enemy if invincible
                audioManager.PlaySFX(audioManager.DeathEnemy);  // Play sound effect
            }
            else
            {
                TakeDamage(1);  // Take damage from destructible objects
                Destroy(destuctable.gameObject);  // Destroy the destructible object
            }
        }

        PowerUp powerUp = collision.GetComponent<PowerUp>();
        if (powerUp)
        {
            audioManager.PlaySFX(audioManager.PowerUp);  // Play power-up sound effect
            if (powerUp.activateShield)
            {
                ActivateShield();  // Activate shield power-up
            }
            if (powerUp.addGuns)
            {
                AddGuns();  // Add guns power-up
            }
            if (powerUp.invincible)
            {
                Invincible();  // Invincibility power-up
            }
            Destroy(powerUp.gameObject);  // Destroy the power-up after collecting it
        }
    }

    void ShowExplosion()
    {
        // Show explosion at the player's position
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 0.1f);  // Destroy the explosion after 0.1 seconds
    }
}