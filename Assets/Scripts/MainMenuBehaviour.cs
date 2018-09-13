using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Load Scene

public class MainMenuBehaviour : MonoBehaviour {

    /// <summary>
    /// Will load a new scene on being called
    /// </summary>
    /// <param name="levelName">Name of the level we want to load</param>
	public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
