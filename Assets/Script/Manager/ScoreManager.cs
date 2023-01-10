using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [ShowInInspector]
    public static int Score = 0;
    [ShowInInspector]
    public static int BestScore;
    [ShowInInspector]
    public static int Star;
    [ShowInInspector] 
    public static int Token;

    private void Start()
    {
        this.RegisterListener(EventID.AddScore, (param) => AddScore((int)param));
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        Star = PlayerPrefs.GetInt("Star", 0);
        Token = PlayerPrefs.GetInt("Token", 0);
        Score = 0;
    }

    private void AddScore(int score)
    {
        Score += score;
        if (BestScore < Score && !GameManager.Instance.InChallenge)
        {
            BestScore = Score;
            PlayerPrefs.SetInt("BestScore", BestScore);
            this.PostEvent(EventID.BestScoreChange);
        }
    }

    public static void ResetScore()
    {
        Score = 0;
    }

    public static void AddStar(int star)
    {
        Star += star;
        PlayerPrefs.SetInt("Star", Star);
        
    }

    [Button("Token")]
    public static void AddToken(int token)
    {
        Token += token;
        PlayerPrefs.SetInt("Token", Token);
    }
}