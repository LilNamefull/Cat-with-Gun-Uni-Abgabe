using UnityEngine;

public class MoveSin : MonoBehaviour
{

    float sinCenterY; // Store the initial Y position of the object
    public float amplitude = 1.5f; // Amplitude of the sine wave (controls the movement range)
    public float frequency = 0.5f; // Frequency of the sine wave (controls the speed of oscillation)
    public bool inverted = false; // Flag to invert the direction of movement

   
    void Start()
    {
        // Save the initial Y position of the object to oscillate around
        sinCenterY = transform.position.y;
    }

    
    void Update()
    {
        // Empty in this case, as FixedUpdate handles the movement
    }

    
    private void FixedUpdate()
    {
        // Get the current position of the object
        Vector2 pos = transform.position;

        // Calculate the sine wave based on the X position and apply amplitude
        float sin = Mathf.Sin(pos.x * frequency) * amplitude;

        // If the inverted flag is true, reverse the direction of the sine wave
        if (inverted)
        {
            sin *= -1; // Invert the movement
        }

        // Set the new Y position by adding the sine value to the initial Y position
        pos.y = sinCenterY + sin;

        // Update the object's position
        transform.position = pos;
    }

}
