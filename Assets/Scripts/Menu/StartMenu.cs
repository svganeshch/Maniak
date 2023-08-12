using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("level_1");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        LoadGame();
    }
}
