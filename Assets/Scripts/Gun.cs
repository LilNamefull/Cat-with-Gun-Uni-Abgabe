using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
   
    public int powerUpLevelRequirement = 0;
    public Bullet bullet;
    Vector2 direction;
    public bool autoShoot = false;

    public float shootIntervalSeconds = 0.5f;
    public float shootDelaySeconds = 0.0f;

    float shootTimer = 0.0f;
    float delayTimer = 0.0f;


    public bool isActive = false;

 
    void Start()
    {
        
    }


    void Update()
    {
        // Calculate the shooting direction based on the object's rotation
        direction = (transform.localRotation * Vector2.right).normalized;

        // If the gun is not active, do nothing
        if (!isActive)
        {
            return;
        }

        // If auto-shooting is enabled
        if (autoShoot)
        {
            // Check if the delay timer has passed the shoot delay
            if (delayTimer >= shootDelaySeconds)
            {
                // If the shoot timer has reached the shoot interval
                if (shootTimer >= shootIntervalSeconds)
                {
                    // Shoot and reset the shoot timer
                    Shoot();
                    shootTimer = 0.0f;
                }
                else
                {
                    // Otherwise, increment the shoot timer
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                // Increment the delay timer before shooting
                delayTimer += Time.deltaTime;
            }
        }
    }

    // Function to instantiate and shoot a bullet
    public void Shoot()
    {
        // Instantiate a bullet at the gun's position
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);

        // Get the Bullet component of the instantiated object
        Bullet goBullet = go.GetComponent<Bullet>();

        // Set the bullet's direction to the calculated direction
        goBullet.direction = direction;
    }

}
