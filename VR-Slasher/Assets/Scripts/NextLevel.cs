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
    private int currentScore;


    void Update()
    {
        // get score from the inventory manger,check score every frame on advancements
        if (LevelToLoad == "map")
        {
            currentScore = KeyInventoryManager.instance.Score;
            print(currentScore);
            // if the score is less then 20 show the text not enough, if currScore == 20 make Canvas invisible
            if (currentScore < 5 && GameManager.instance.gamesWon < 1)
            {
                Portal.SetActive(false);
            }
            else
            {
                Portal.SetActive(true);
            }
                
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
