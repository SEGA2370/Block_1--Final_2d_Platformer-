using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("GameOver")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause screen
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    #region Game Over Logic

    // Activate Game Over Screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
        Time.timeScale = 0f; // Pause the game
    }

    // GameOver Functions
    public void Restart()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Pause Logic

    /// <summary>
    /// Call this from a UI Button OnClick
    /// </summary>
    public void TogglePause()
    {
        bool isPaused = pauseScreen.activeSelf;
        PauseGame(!isPaused);
    }

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        // Pause or unpause the game
        Time.timeScale = status ? 0f : 1f;
    }

    #endregion
}
