using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehaviour : MainMenuBehaviour
{

    public static bool paused;

    [Tooltip("Reference to the pause menu to turn on or off")]
    public GameObject pauseMenu;

    /// <summary>
    /// Reloads our current level, restarting the game
    /// </summary>
	public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Will turn our pause menu on or off
    /// </summary>
    /// <param name="isPaused">If set to <c>true</c> is paused.</param>
    public void SetPauseMenu(bool isPaused)
    {
        paused = isPaused;

        // if the game is paused, timeScale = 0, otherwise 1
        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }

    void Start()
    {
        paused = false;
        SetPauseMenu(paused);
    }
}
