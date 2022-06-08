using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
public class KeyInventoryManager : MonoBehaviour
{
    public static KeyInventoryManager instance;
    public Text ScoreText;
    public int Score { get; private set; } = 0;
    private void Awake()
    {
        instance = this;
    }
    // update score +1 
    public void AddPoint()
    {
        instance.Score += 1;
        ScoreText.text = Score.ToString() + "/5";
    }
}
