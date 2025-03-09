using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign this in the Inspector
    public Button resumeButton; // Assign this in the Inspector
    public Button quitButton; // Assign this in the Inspector

    private bool isPaused = false;

    void Start()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Ensure the pause menu is hidden at the start
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    void QuitGame()
    {
        Application.Quit(); // Close the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing the game in the editor
#endif
    }
}
