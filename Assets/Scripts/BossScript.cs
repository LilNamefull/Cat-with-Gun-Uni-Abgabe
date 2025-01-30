using UnityEngine;
using UnityEngine.UI;


public class BossScript : MonoBehaviour
    {
    public float moveSpeed = 5f;          
    public float moveHeight = 3f;          
    public float shootCooldown = 1f;       
    public GameObject bulletPrefab;        
    public GameObject rocketPrefab;        
    public Transform shootPoint;           
    private Vector3 startPosition;         

    private float shootTimer = 0f;         
    public float shootInterval = 1f;      
    public float spreadAngle = 45f;       
    public int maxHealth = 80;           

    private int health;                    
    public Slider healthBar;               
    public int pelletCount = 3;          
    public int rocketCount = 1;            

    public GameManager gameManager;        

    public GameObject explosionPrefab;     
    private bool phaseChanged = false;     

    public AudioSource backgroundMusic;    

    private void Awake()
    {
        stopMusic();                     // Stop background music when boss awakens
        health = maxHealth;               // Set the initial health to maximum
    }

    public void stopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();      // Stop the background music
        }
    }

    private void Start()
    {
        startPosition = transform.position;  // Save the starting position of the boss

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;   // Set the health bar's maximum value to max health
            healthBar.value = health;         // Set the health bar's current value to the current health
        }
    }

    private void Update()
    {
        if (health > 40)
        {
            StandardBehavior();  // Perform standard behavior if health is above 40
        }
        else if (!phaseChanged)
        {
            phaseChanged = true;  // Change to a new phase when health drops below 40
            StartNewPhase();
        }

        if (phaseChanged)
        {
            NewBehavior();  // Perform new behavior after phase change
        }
    }

    private void StandardBehavior()
    {
        MoveBoss();  // Move the boss
        shootTimer -= Time.deltaTime;  // Countdown the shoot timer

        if (shootTimer <= 0f)
        {
            ShootShotgun();  // Perform shotgun-style shooting
            ShootRocket();    // Shoot a rocket
            shootTimer = shootCooldown;  // Reset the shoot timer
        }
    }

    private void StartNewPhase()
    {
        Debug.Log("Boss is switching to a new phase!");

        moveSpeed *= 1.5f;  // Increase boss's movement speed
        shootCooldown /= 2;  // Decrease the shoot cooldown (boss shoots faster)
    }

    private void NewBehavior()
    {
        // Zigzag movement pattern
        float zigzag = Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(transform.position.x, zigzag, transform.position.z);

        shootTimer -= Time.deltaTime;  // Countdown the shoot timer

        if (shootTimer <= 0f)
        {
            BulletHellAttack();  // Perform a Bullet Hell attack pattern
            shootTimer = shootCooldown;  // Reset the shoot timer
        }
    }

    private void BulletHellAttack()
    {
        int bulletCount = 15;  // Number of bullets to fire
        float bulletSpeed = 5f;  // Speed of the bullets

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);  // Evenly spread bullets in a circle
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = -bullet.transform.right;  // Bullets move along their rotation direction
            rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);  // Apply force to move the bullets
        }
    }

    private void MoveBoss()
    {
        // Move the boss from right to left, then up and down
        if (transform.position.x > startPosition.x - 10f)  // Boss comes from the right
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);  // Move boss left
        }
        else
        {
            // After reaching the target X position, the boss moves up and down
            float verticalMove = Mathf.PingPong(Time.time * moveSpeed, moveHeight) - (moveHeight / 2f);
            transform.position = new Vector3(transform.position.x, startPosition.y + verticalMove, transform.position.z);
        }

        if (health < 25)
        {
            // More intense zigzag movement when health is low
            ZickZackMovement();
        }
    }

    private void ZickZackMovement()
    {
        float zigzagX = Mathf.Sin(Time.time * moveSpeed * 2f) * (moveHeight * 0.8f);  // Fast horizontal movement
        float zigzagY = Mathf.Cos(Time.time * moveSpeed * 3f) * moveHeight;  // Fast vertical movement

        transform.position = new Vector3(startPosition.x + zigzagX, startPosition.y + zigzagY, transform.position.z);
    }

    private void Shoot()
    {
        ShootShotgun();  // Perform shotgun shooting
        ShootRocket();   // Shoot a rocket
    }

    private void ShootShotgun()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // Calculate a random spread angle for shotgun pellets
            float spread = Random.Range(-spreadAngle, spreadAngle);

            Quaternion rotation = Quaternion.Euler(0, 0, spread);  // Apply rotation based on the spread
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = -bullet.transform.right;  // Set the direction of the bullet
            rb.AddForce(direction * 5f, ForceMode2D.Impulse);  // Apply force to the bullet
        }
    }

    private void ShootRocket()
    {
        for (int i = 0; i < rocketCount; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, shootPoint.position, shootPoint.rotation);  // Instantiate a rocket

            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            Vector2 rocketDirection = -shootPoint.right;  // Set the direction of the rocket
            rb.AddForce(rocketDirection * 20f, ForceMode2D.Impulse);  // Apply force to move the rocket
        }
    }

    private bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Do nothing if the boss is already dead

        health -= damage;  // Subtract the damage from the boss's health
        Debug.Log($"Boss took {damage} damage. Remaining health: {health}");

        if (healthBar != null)
        {
            healthBar.value = health;  // Update the health bar
        }

        if (health <= 0)
        {
            isDead = true;  // Mark the boss as dead
            Die();          // Call the Die method
        }
    }

    private void Die()
    {
        GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ShowVictoryUI(Level.instance.GetScore());  // Show victory UI when the boss dies
        }

        Destroy(gameObject);  // Destroy the boss game object
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);  // Ignore collision with enemies
        }

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);  // Take damage from the player's bullet
            }

            Destroy(collision.gameObject);  // Destroy the bullet
        }

        if (collision.gameObject.CompareTag("BossBullet"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);  // Ignore collision with other boss bullets
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);  // Ignore collision with enemy bullets
            return;
        }
    }
}
