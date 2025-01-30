using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public AudioClip shootingSound; 
    public GameObject bulletPrefab; 
    public Transform shootPoint; 
    public float shootInterval = 2f; 

    private AudioManager audioManager;
    private float shootTimer; 

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();  // Get the AudioManager instance to play sounds
        shootTimer = shootInterval; // Initialize the shoot timer with the interval value
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime; // Decrease the shoot timer by the time that has passed since the last frame

        // Only shoot if the timer is up and the enemy is visible in the camera's view
        if (shootTimer <= 0f && IsVisibleFromCamera(Camera.main))
        {
            Shoot();  // Call the Shoot method to fire
            shootTimer = shootInterval; // Reset the shoot timer to the interval
        }
    }

    private void Shoot()
    {
        // Instantiate the bullet prefab at the shoot point's position with the shoot point's rotation
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Play the shooting sound if the AudioManager and sound are available
        if (audioManager != null && shootingSound != null)
        {
            audioManager.PlayEnemySound(shootingSound); // Play the shooting sound effect
        }
    }

    private bool IsVisibleFromCamera(Camera camera)
    {
        // Calculate the camera's frustum planes and test if the enemy's collider is within the camera's view
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds); // Return true if the enemy is within the camera's view
    }
}

