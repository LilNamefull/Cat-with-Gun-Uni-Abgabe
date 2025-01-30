using UnityEngine;

public class EnemyFootstep : MonoBehaviour
{
    public AudioClip footstepSound; 
    public float footstepInterval = 0.5f; 
    public float movementThreshold = 0.1f; 

    private AudioManager audioManager;
    private Rigidbody2D rb;
    private float footstepTimer;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Get the AudioManager instance to play sounds
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component to track movement velocity
    }

    private void Update()
    {
        // Log the current velocity and threshold for debugging purposes
        Debug.Log($"Velocity: {rb.linearVelocity.magnitude}, Threshold: {movementThreshold}");

        // Play the footstep sound only if the enemy is moving fast enough and is visible in the camera
        if (rb != null && rb.linearVelocity.magnitude > movementThreshold && IsVisibleFromCamera(Camera.main))
        {
            footstepTimer -= Time.deltaTime; // Decrease the footstep timer by the time passed since last frame

            // If the timer is up, play the footstep sound and reset the timer
            if (footstepTimer <= 0f)
            {
                Debug.Log("Footstep condition met, timer finished!");
                PlayFootstep();
                footstepTimer = footstepInterval; // Reset the timer to the interval value
            }
        }
        else
        {
            Debug.Log("No movement or below threshold, no footstep sound.");
            footstepTimer = 0f; // Reset the timer if the enemy is not moving fast enough or is not visible
        }
    }

    private void PlayFootstep()
    {
        Debug.Log("Playing footstep sound!");
        if (audioManager != null && footstepSound != null)
        {
            audioManager.PlayEnemySound(footstepSound); // Play the footstep sound if both the AudioManager and the sound are set
        }
        else
        {
            Debug.LogWarning("AudioManager or footstep sound is not assigned!");
        }
    }

    private bool IsVisibleFromCamera(Camera camera)
    {
        // Calculate the camera's frustum planes and check if the enemy's collider is within the camera's view
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds); // Return true if the enemy is visible
    }
}

