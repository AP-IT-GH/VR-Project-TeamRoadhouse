using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    // level thats has to be loaded (needs to be in the build)
    public string LevelToLoad = "";

    public GameObject Portal;

    // current player score
    private int keyScore;
    private int gameScore;

    void Start()
    {
        Portal.SetActive(false);
    }

    void Update()
    {
            keyScore = KeyInventoryManager.instance.Score;
            gameScore = BallGameManager.instance.gamesWon;
            if (keyScore == 5 && gameScore == 1)
            {
                Portal.SetActive(true);
            }
    }

    public void Load()
    {
        if (LevelToLoad != "")
        {
            SceneManager.LoadScene(LevelToLoad);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
