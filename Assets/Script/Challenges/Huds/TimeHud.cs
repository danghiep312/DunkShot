using System;
using TMPro;
using UnityEngine;

public class TimeHud : MonoBehaviour
{
    public TextMeshProUGUI titleLevel;
    public TextMeshProUGUI second;
    public TextMeshProUGUI milliSecond;
    public TextMeshProUGUI hoopRemaining;

    public int totalHoop;
    public float timeRemaining;
    public bool isGameStarted;

    private TimeSpan timeSpan;
    private void OnEnable()
    {
        timeRemaining = LevelManager.Instance.currentLv.target;
        totalHoop = LevelManager.Instance.currentLv.totalHoop;
        isGameStarted = false;
    }

    private void Start()
    {
        this.RegisterListener(EventID.StartChallenge, (param) => OnStartChallenge());
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        this.RegisterListener(EventID.BackFromChallenge, (param) => BackFromChallenge());
        this.RegisterListener(EventID.RestartLevel, (param) => OnRestartLevel());
    }

    private void BackFromChallenge()
    {
        isGameStarted = false;
        timeRemaining = 5;
    }

    private void OnStartChallenge()
    {
        isGameStarted = true;
    }

    private void Update()
    {
        if (isGameStarted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else if (timeRemaining < 0)
            {
                timeRemaining = 0;
                this.PostEvent(EventID.TimeOut);
            }
            
        }
        titleLevel.text = "CHALLENGE " + int.Parse(LevelManager.Instance.currentLv.id.ToString().Substring(1, 2));
        hoopRemaining.text = LevelManager.Instance.hoopPassed + "/" + totalHoop + " HOOPS";
        timeSpan = TimeSpan.FromSeconds(timeRemaining);
        var time = timeSpan.ToString(@"ss\:ff");
        second.text = time.Split(':')[0] + ":";
        milliSecond.text = time.Split(':')[1];
    }

    public void ResetStatus()
    {
        timeRemaining = LevelManager.Instance.currentLv.target;
        isGameStarted = false;
    }

    private void OnGameOver()
    {
        isGameStarted = false;
    }

    private void OnRestartLevel()
    {
        timeRemaining = LevelManager.Instance.currentLv.target;
        totalHoop = LevelManager.Instance.currentLv.totalHoop;
        isGameStarted = false;
    }
}
