using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGameManager : MonoBehaviour
{
    public static BallGameManager instance;
    
    public int amountOfCans = 9;
    public GameObject winCanvas;
    public bool ballGameWon;
    public int gamesWon;
    private int currentScore;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gamesWon = 0;
        currentScore = 0;
        winCanvas.SetActive(false);
    }

    void Update()
    {
        currentScore = CanPointsManager.instance.points;
        if (currentScore == amountOfCans)
        {
            if (!ballGameWon)
            {
                ballGameWon = true;
                gamesWon++;
                winCanvas.SetActive(true);
            }
        }
    }
}
