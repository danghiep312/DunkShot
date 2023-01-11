using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public static string LEVEL_PATH = "LevelPrefabs/";
    public Level[] levels;
    
    public GameObject currentLevel;
    public Level currentLv;
    public int hoopPassed;
    public int totalBounce;
    public int totalToken;
    public int hp;

    public GameObject ball;
    private void Start()
    {
        this.RegisterListener(EventID.GoHome, (param) => GoHome());
        this.RegisterListener(EventID.ReachFinishHoop, (param) => ReachFinishHoop());
        this.RegisterListener(EventID.PassHoopChallenge, (param) => PassHoopChallenge());
        this.RegisterListener(EventID.TimeOut, (param) => OnTimeOut());
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        this.RegisterListener(EventID.GetToken, (param) => OnGetToken());
        this.RegisterListener(EventID.BackFromChallenge, (param) => OnBackFromChallenge());

        levels = Resources.LoadAll<Level>("Levels");
    }

    private void OnBackFromChallenge()
    {
        hoopPassed = 0;
        totalBounce = 0;
        totalToken = 0;
        hp = 3;
    }

    private void OnGetToken()
    {
        totalToken++;
    }

    private void OnGameOver()
    {
        if (!GameManager.Instance.InChallenge) return;
        if (currentLv.type.Equals("NewBall") |
            currentLv.type.Equals("NoAim"))
        {
            if (hp > 0) hp--;
            if (hp <= 0)
            {
                UIManager.Instance.ShowUncompleteChallengePanel();
                if (currentLv.type.Equals("NewBall"))
                {
                    PlayerPrefs.SetInt("4" + currentLv.id.ToString().Substring(1, 2), 1);
                }
            }
        }
        else
        {
            UIManager.Instance.ShowUncompleteChallengePanel();
        }
    }

    private void OnHoopPassed()
    {
        if (ball == null) ball = Camera.main.GetComponent<CamController>().ball;
        int tmp = ball.GetComponent<Ball>().Bounce;
        if (tmp > 0)
        {
            totalBounce += tmp;
        }
    }

    private void OnTimeOut()
    {
        if (ball == null) ball = Camera.main.GetComponent<CamController>().ball;
        if (ball.transform.parent != null)
        {
            this.PostEvent(ball.transform.parent.name.Contains("Finish")
                ? EventID.CompleteChallenge
                : EventID.UncompleteChallenge);
        } 
    }

    private void PassHoopChallenge()
    {
        hoopPassed++;
    }

    private void ReachFinishHoop()
    {
        if (currentLv.type.Equals("Time") | currentLv.type.Equals("NewBall") | currentLv.type.Equals("NoAim"))
        {
            PlayerPrefs.SetInt("Level" + currentLv.id, 1);
            if (int.Parse(currentLv.id.ToString().Substring(1, 2)) ==
                GetNumberLevelOfType(int.Parse(currentLv.id.ToString().Substring(0, 1))))
            {
                PlayerPrefs.SetInt(currentLv.type + "Complete", 1);
            }
            this.PostEvent(EventID.CompleteChallenge);
            PlayerPrefs.SetInt("4" + currentLv.id.ToString().Substring(1, 2), 1);
        }
        else switch (currentLv.type)
        {
            case "Score":
                this.PostEvent(ScoreManager.Score >= currentLv.target
                    ? EventID.CompleteChallenge
                    : EventID.UncompleteChallenge);
                PlayerPrefs.SetInt("Level" + currentLv.id, ScoreManager.Score >= currentLv.target ? 1 : 0);
                
                break;
            case "Bounce":
                this.PostEvent(totalBounce >= currentLv.target ? EventID.CompleteChallenge : EventID.UncompleteChallenge);
                PlayerPrefs.SetInt("Level" + currentLv.id, totalBounce >= currentLv.target ? 1 : 0);
                break;
            case "Collect":
                this.PostEvent(totalToken >= currentLv.target ? EventID.CompleteChallenge : EventID.UncompleteChallenge);
                PlayerPrefs.SetInt("Level" + currentLv.id, totalToken >= currentLv.target ? 1 : 0);
                break;
        }
        
    }

    /// <summary>
    /// type = 1, 2, 3, 4, 5, 6 is new ball challenge, collect, time, score, bounce and no aim
    /// </summary>
    /// <param name="type"></param>
    public void GoToChallengeLevel(int type)
    {
        hp = 3;
        var typeName = type switch
        {
            1 => "NewBall",
            2 => "Collect",
            3 => "Time",
            4 => "Score",
            5 => "Bounce",
            6 => "NoAim",
            _ => ""     
        };
        var level = GetCurrentLevelToPlay(type);
        Debug.Log(level);
        if (level == -1)
        {
            // TODO: https://www.youtube.com/watch?v=BTeywmc3-zM (make effect for this case)
            Debug.Log("No level to play");
            return;
        }
        var levelPrefab = Resources.Load<GameObject>(LEVEL_PATH + level);
        currentLevel = Instantiate(levelPrefab);
        currentLv = Resources.Load<Level>("Levels/" + level);
        this.PostEvent(EventID.GoToChallenge, typeName);
    }

    public void PlayLevel()
    {
        this.PostEvent(EventID.PlayChallenge);
        ball = GameObject.FindGameObjectWithTag("Ball");
        hoopPassed = 0;
        totalBounce = 0;
        totalToken = 0;
        hp = 3;
        ScoreManager.ResetScore();
    }

    public GameObject GetFirstHoopOfCurrentLevel()
    {
        return (from Transform t in currentLevel.transform where t.gameObject.name.Contains("1st") select t.gameObject)
            .FirstOrDefault();
    }

    public void RestartLevel()
    {
        hoopPassed = 0;
        totalBounce = 0;
        totalToken = 0;
        hp = 3;
        this.PostEvent(EventID.RestartLevel);
        var level = int.Parse(currentLv.id.ToString().Substring(1, 2));
        var levelPrefab = Resources.Load<GameObject>(LEVEL_PATH + currentLv.id.ToString().Substring(0,1) + $"{level:00}");
        ball.transform.parent = null;
        ball.GetComponent<Ball>().ResetStatus();
        DestroyImmediate(currentLevel);
        currentLevel = Instantiate(levelPrefab);
        PlayLevel();
        Time.timeScale = 1;
    }

    private int GetCurrentLevelToPlay(int type)
    {
        var typeName = type switch
        {
            1 => "NewBall",
            2 => "Collect",
            3 => "Time",
            4 => "Score",
            5 => "Bounce",
            6 => "NoAim",
            _ => "" 
        };
        foreach (var level in levels)
        {
            if (!level.type.Equals(typeName)) continue;
            if (PlayerPrefs.GetInt("Level" + level.id) == 0)
            {
                return level.id;
            }
        }

        return -1;
    }

    private void GoHome()
    {
        try
        {
            currentLevel.SetActive(false);
            DestroyImmediate(currentLevel);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public int GetNumberCompleteLevel(int type)
    {
        var typeName = type switch
        {
            1 => "NewBall",
            2 => "Collect",
            3 => "Time",
            4 => "Score",
            5 => "Bounce",
            6 => "NoAim",
            _ => "" 
        };
        for (int i = levels.Length - 1; i >= 0; i--)
        {
            if (levels[i].type.Equals(typeName) && PlayerPrefs.GetInt("Level" + levels[i].id) == 1)
            {
                return levels[i].id - type * 100;
            }
        }

        return 0;
    }

    public int GetNumberLevelOfType(int type)
    {
        var typeName = type switch
        {
            1 => "NewBall",
            2 => "Collect",
            3 => "Time",
            4 => "Score",
            5 => "Bounce",
            6 => "NoAim",
            _ => "" 
        };
        return levels.Count(level => level.type.Equals(typeName));
    }
}
