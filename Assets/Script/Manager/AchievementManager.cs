using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class AchievementManager : Singleton<AchievementManager>
{
    public int[] storage;
    /// <summary>
    /// 0 play game, 1 bounce, 2 perfect streak max, 3 total star, 4 best score
    /// </summary>
    public int numOfParam;
    private void Start()
    {
        storage = new int[numOfParam];
        for (var i = 0; i < numOfParam; i++)
        {
            storage[i] = PlayerPrefs.GetInt(GetKeyStorage(i));
        }
        Check();
        this.RegisterListener(EventID.PlayGame, (param) => OnPlayGame());
        this.RegisterListener(EventID.Bounce, (param) => OnBounce());
        this.RegisterListener(EventID.BestScoreChange, (param) => OnBestScoreChange());
        this.RegisterListener(EventID.GetStar, (param) => OnGetStar((int)param));
        this.RegisterListener(EventID.ReachPerfect, (param) => OnReachPerfect());
        this.RegisterListener(EventID.WatchVideo, (param) => OnWatchVideo());
    }

    private void OnWatchVideo()
    {
        Check();
    }

    private void OnReachPerfect()
    {
        var index = GetIndexOfKey("PerfectStreak");
        storage[index] = Math.Max(GameManager.Instance.GetStreak(), storage[index]);
        PlayerPrefs.SetInt("PerfectStreak", storage[index]);
        Check();
    }

    private void OnGetStar(int param)
    {
        var index = GetIndexOfKey("Star");
        storage[index] += param;
        PlayerPrefs.SetInt("Star", storage[index]);
        Check();
    }

    private void OnPlayGame()
    {
        var index = GetIndexOfKey("PlayGame");
        PlayerPrefs.SetInt("PlayGame", ++storage[index]);
        Check();
    }

    private void OnBounce()
    {
        var index = GetIndexOfKey("Bounce");
        PlayerPrefs.SetInt("Bounce", ++storage[index]);
        Check();
    }

    private void OnBestScoreChange()
    {
        var index = GetIndexOfKey("BestScore");
        storage[index] = ScoreManager.BestScore;
        PlayerPrefs.SetInt("BestScore", storage[index]);
        Check();
    }
    
    private void OnApplicationQuit()
    {
        Array.ForEach(storage, (x) => PlayerPrefs.SetInt(GetKeyStorage(x), x));
    }

    private string GetKeyStorage(int index)
    {
        return index switch
        {
            0 => "PlayGame",
            1 => "Bounce",
            2 => "PerfectStreak",
            3 => "Star",
            4 => "BestScore",
            _ => "PlayGame"
        };
    }

    private int GetIndexOfKey(string key)
    {
        return key switch
        {
            "PlayGame" => 0,
            "Bounce" => 1,
            "PerfectStreak" => 2,
            "Star" => 3,
            "BestScore" => 4,
            _ => 0
        };
    }

    private void CheckCondition(int param, int comparisonTarget, int id)
    {
        if (param >= comparisonTarget)
        {
            PlayerPrefs.SetInt(id.ToString(), 1);
        }
    }

    public void Check()
    {
        var s = GameSpriteMachine.Instance.skins;
        foreach (var skin in s)
        {
            if (Regex.IsMatch(skin.id.ToString(), @"^2.{2}$"))
            {
                CheckCondition(PlayerPrefs.GetInt("Watch" + skin.id), skin.price, skin.id);
            }
            else if (Regex.IsMatch(skin.id.ToString(), @"^3.{2}$"))
            {
                CheckCondition(storage[skin.paramToCheck], skin.price, skin.id);
            }
        }
    }

    public int GetCurrentValueProgress(Skin skin)
    {
        return storage[skin.paramToCheck];
    }
}
