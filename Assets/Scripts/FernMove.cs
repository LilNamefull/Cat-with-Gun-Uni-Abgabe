using UnityEngine;

public class FernMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float moveHeight = 3f;
    private Vector3 startPosition;        

 
    void Start()
    {
        startPosition = transform.position;  // Save the initial position of the enemy
    }

   
    void Update()
    {
        // The enemy moves to the left until it reaches x = 4
        if (transform.position.x > 4f)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); // Move left over time
        }
        else
        {
            // Once x = 4 is reached, the enemy starts moving up and down
            float verticalMove = Mathf.PingPong(Time.time * moveSpeed, moveHeight) - (moveHeight / 2f);
            // Fix the X position to 4 and adjust the Y position based on PingPong to create the vertical movement
            transform.position = new Vector3(4f, startPosition.y + verticalMove, transform.position.z);
        }
    }
}
