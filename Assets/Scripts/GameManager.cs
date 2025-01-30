using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // The UI elements that will be shown when the game is over or the player wins
    public GameObject gameOverUI;
    public GameObject victoryUI;

    // The text UI element that will display the player's score
    public TMP_Text scoreText;

    // The background music audio source
    public AudioSource backgroundMusic;

    // The player object and its associated movement script
    public PlayerMovement player;

    // The player's score value
    public int playerScore;

    // The text UI element that will display the victory score
    public TMP_Text victoryScoreText;

    // Function to handle the game over scenario
    public void gameOver()
    {
        gameOverUI.SetActive(true); // Display the game over UI
        int finalScore = Level.instance.GetScore(); // Get the final score from the level manager
        scoreText.text = "Score: " + finalScore; // Update the score text UI
        Time.timeScale = 0; // Pause the game by setting the time scale to 0
    }

    // Function to restart the game after game over
    public void restart()
    {
        Debug.Log("Reset is being executed...");
        stopMusic(); // Stop the background music
        Time.timeScale = 1; // Resume normal time flow
        GameObject playerObject = GameObject.FindWithTag("Player"); // Find the player object
        if (playerObject != null)
        {
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>(); // Get the player's movement script
            if (player != null)
            {
                player.ResetPlayer(); // Reset the player’s state
            }
        }
        Level.instance.ResetLevel(); // Reset the level
        SceneManager.LoadScene("Level1"); // Reload the first level
    }

    // Function to return to the main menu
    public void mainMenu()
    {
        stopMusic(); // Stop the background music
        Time.timeScale = 1; // Resume normal time flow
        Level.instance.ResetLevel(); // Reset the level
        GameObject playerObject = GameObject.FindWithTag("Player"); // Find the player object
        if (playerObject != null)
        {
            PlayerMovement player = playerObject.GetComponent<PlayerMovement>(); // Get the player's movement script
            if (player != null)
            {
                player.ResetPlayer(); // Reset the player’s state
            }
        }
        if (Level.instance != null)
        {
            Destroy(Level.instance.gameObject); // Destroy the level manager to prevent duplication
        }
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Function to stop the background music
    public void stopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // Stop the background music audio source
        }
    }

    // Function to quit the game
    public void Quit()
    {
        Application.Quit(); // Exit the application
        Debug.Log("Quit");
    }

    // Function to display the victory screen
    public void ShowVictoryUI(int finalScore)
    {
        Debug.Log("ShowVictoryScreen() wird aufgerufen");

        if (victoryUI == null)
        {
            Debug.LogError("Victory UI wurde nicht zugewiesen!"); // Log an error if victory UI is not assigned
            return;
        }

        victoryUI.SetActive(true); // Show the victory UI
        victoryScoreText.text = "Score: " + finalScore; // Display the final score
        Time.timeScale = 0; // Pause the game by setting the time scale to 0
    }
}
    

