using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanPointsManager : MonoBehaviour
{
    public static CanPointsManager instance;
    
    public Text pointsText;
    public int points = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pointsText.text = "Points: " + points;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Can"))
        {
            points++;
            pointsText.text = "Points: " + points;
        }
    }
}
