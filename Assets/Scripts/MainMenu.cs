using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This method loads the next scene in the build index
    public void PlayGame()
    {
        // Load the scene with the next build index (next scene in the list)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // This method quits the game application
    public void QuitGame()
    {
        // Closes the application (works only in a built version, not in the editor)
        Application.Quit();
    }
}
