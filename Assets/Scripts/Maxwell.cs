using UnityEngine;

public class Maxwell : MonoBehaviour
{

    public float moveSpeed = 5f;          //Movespeed


    public float moveHeight = 3f;         // Movehight

    // Private variable to store the enemy's starting position
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store the enemy's initial position when the game starts
        startPosition = transform.position;  // Safes the startposition
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy's X position is greater than 15 (i.e., it's moving to the right)
        if (transform.position.x > 15f)
        {
            // Move the enemy to the left by translating it to the right (negative direction)
            // This keeps the enemy moving left until the X position reaches 4
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Once the enemy reaches x = 15, start making it move up and down
            // The Mathf.PingPong function creates a repeating motion between 0 and moveHeight
            float verticalMove = Mathf.PingPong(Time.time * moveSpeed, moveHeight) - (moveHeight / 2f);

            // Update the enemy's position, keeping the X position fixed at 15
            // The Y position will oscillate based on verticalMove
            transform.position = new Vector3(15f, startPosition.y + verticalMove, transform.position.z);  // Locks the x kordinate on x = 15
        }

    }
}
