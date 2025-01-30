using UnityEngine;

using System.Collections;
using System.Collections.Generic;
public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array of power-up prefabs to spawn
    public float spawnIntervalMin = 3f; // Minimum time interval between spawns (in seconds)
    public float spawnIntervalMax = 7f; // Maximum time interval between spawns (in seconds)
    public float spawnYRange = 1.3f;    // The vertical range within which the power-up can spawn
    public float powerUpSpeed = 2f;     // The speed at which the power-up moves to the left
    public float destroyXPosition = -14f; // The X position at which the power-up is destroyed (out of the screen)

    private void Start()
    {
        // Start the coroutine to spawn power-ups
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        // Infinite loop to continuously spawn power-ups
        while (true)
        {
            // Wait for a random time between the specified min and max spawn intervals
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // Select a random power-up from the powerUpPrefabs array
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject selectedPowerUp = powerUpPrefabs[randomIndex];

            // Generate a random Y position within the spawn range
            float randomY = Random.Range(-spawnYRange, spawnYRange);

            // Spawn the selected power-up at the calculated position
            Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
            GameObject spawnedPowerUp = Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);

            // Make the power-up move to the left by applying linear velocity
            spawnedPowerUp.GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * powerUpSpeed;

            // Destroy the power-up when it moves past the defined X position
            Destroy(spawnedPowerUp, Mathf.Abs((transform.position.x - destroyXPosition) / powerUpSpeed));
        }
    }
}
