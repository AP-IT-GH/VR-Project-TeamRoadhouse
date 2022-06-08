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
    void Update()
    {
            keyScore = KeyInventoryManager.instance.Score;
            gameScore = BallGameManager.instance.gamesWon;
            print(keyScore);
            if (keyScore < 5 && gameScore < 1)
            {
                Portal.SetActive(false);
            }
            else
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

}
