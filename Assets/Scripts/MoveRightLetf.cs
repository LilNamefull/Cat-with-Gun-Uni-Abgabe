using UnityEngine;

public class MoveRightLetf : MonoBehaviour
{
    public float moveSpeed = 5;

    // Reference to an AudioManager object (though it's not being used in the current script)
    AudioManager audioManager;

   
    void Start()
    {
     
    }

    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Get the current position of the object
        Vector2 pos = transform.position;

        // Move the object to the left by decreasing its X position over time
        pos.x -= moveSpeed * Time.fixedDeltaTime;

        // If the object goes off-screen (X position is less than -17), destroy it
        if (pos.x < -17)
        {
            Destroy(gameObject); // Destroy the object when it moves out of the view
        }

        // Update the object's position to reflect the new value
        transform.position = pos;
    }

}
