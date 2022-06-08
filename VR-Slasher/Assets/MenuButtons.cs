using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Scenes/map");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
