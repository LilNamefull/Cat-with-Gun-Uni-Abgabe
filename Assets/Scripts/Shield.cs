using UnityEngine;

public class Shield : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component that controls the visibility of the shield
    public float blinkInterval = 0.2f;     // Time between each blink (in seconds)

    private bool isBlinking = false;        // A flag to check if the shield is currently blinking

    // Activates the shield with a blinking effect
    public void ActivateShield(float duration)
    {
        if (!isBlinking)  // Check if the shield isn't already blinking
        {
            gameObject.SetActive(true);  // Make the shield GameObject visible
            StartCoroutine(BlinkShield(duration));  // Start the blinking coroutine
        }
    }

    // Deactivates the shield immediately
    public void DeactivateShield()
    {
        StopAllCoroutines();           // Stop any running coroutines (including the blinking one)
        spriteRenderer.enabled = false;  // Hide the shield (make the sprite invisible)
        gameObject.SetActive(false);   // Deactivate the shield GameObject
        isBlinking = false;            // Reset the blinking flag
    }

    // Coroutine that makes the shield blink for a specified duration
    private System.Collections.IEnumerator BlinkShield(float duration)
    {
        isBlinking = true;             // Set the blinking flag to true
        float timer = 0f;              // Timer to track the duration of the blinking effect

        while (timer < duration)      // Loop while the shield should still be blinking
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;  // Toggle the visibility of the shield (blink)
            yield return new WaitForSeconds(blinkInterval);    // Wait for the specified blink interval
            timer += blinkInterval;                            // Increase the timer
        }

        spriteRenderer.enabled = false;  // After the blinking ends, ensure the shield is invisible
        gameObject.SetActive(false);    // Deactivate the shield GameObject
        isBlinking = false;             // Reset the blinking flag
    }
}