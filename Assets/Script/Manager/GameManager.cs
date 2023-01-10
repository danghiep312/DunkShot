using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("GameObject")]
    public Ball ball;
    public CamController cam;
    public GameObject endless;
    public GameObject firstHoop;
    
    [Header("Size of screen")]
    public float HorizontalScreen;
    public float VerticalScreen;
    
    [Header("State of game")]
    public bool GameOver;
    public bool InHome;
    public bool InChallenge;

    [Header("State of sfx")]
    public bool soundOn;
    public bool vibrationOn;
    public bool nightModeOn;

    public bool canContinue;
    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(Vector2.one);
        HorizontalScreen = edgeVector.x * 2;
        VerticalScreen = edgeVector.y * 2;
        
        soundOn = PlayerPrefs.GetInt("soundOn", 1) == 1;
        vibrationOn = PlayerPrefs.GetInt("vibrationOn", 1) == 1;
        nightModeOn = PlayerPrefs.GetInt("nightModeOn", 0) == 1;
        
        // Spawn ball when game starts
        ball = ObjectPooler.Instance.Spawn("Ball").GetComponent<Ball>();
        ball.gameObject.transform.position = Vector3.right * -2.2f + Vector3.up * 1f;
     
        //Debug.Break();
    }

    
    private void Start()
    {
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        this.RegisterListener(EventID.GoToChallenge, (param) => OnGoToChallenge());
        this.RegisterListener(EventID.PlayChallenge, (param) => OnPlayChallenge());

        PredictionManager.Instance.CopyAllObstacles();
        GameOver = false;
        canContinue = true;
        InHome = true;
    }
    

    public void GoHome()
    {
        GameOver = false;
        InChallenge = false;
        canContinue = true;
        InHome = true;
        UIManager.Instance.GoHome();
        ObjectPooler.Instance.ResetStatus();
        cam.MoveCameraToOriginal();
        ScoreManager.ResetScore();
        HoopSpawner.Instance.CallStart();
        Time.timeScale = 1;
        this.PostEvent(EventID.GoHome);
    }

    public void Continue()
    {
        //cam.ActiveVirtualCamera();
//        Debug.Log("Continue");
        canContinue = false;
        if (ball.gameObject != null)
        {
            ball.gameObject.transform.position =
                HoopSpawner.Instance.GetCurrentHoop().transform.position + Vector3.up * 2f;
            ball.SetBallStatus(false);
            ball.SetBallStatus(true);
        }
        HoopSpawner.Instance.GetCurrentHoop().GetComponent<Hoop>().ResetRotation();
        if (InChallenge)
        {
            UIManager.Instance.ResetStatusTimeHud();
        }
    }

    private void OnGameOver()
    {
        if (canContinue) return;
        GameOver = true;
    }

    private void OnGoToChallenge()
    {
        ObjectPooler.Instance.ResetStatus();
        firstHoop.SetActive(false);
    }
    
    private void OnPlayChallenge()
    {
        InChallenge = true;
        ball.gameObject.SetActive(true);
        ball.Spawn(LevelManager.Instance.GetFirstHoopOfCurrentLevel().transform.position + Vector3.up * 2f);
        //Debug.Break();
    }

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(0);
    }
    

    #region Get Bounce And Streak

    public int GetStreak()
    {
        return ball.Streak;
    }

    public int GetBounce()
    {
        return ball.Bounce;
    }

    #endregion
    

    #region ToggleSettingFunc

    public void ToggleSound()
        {
            soundOn = !soundOn;
            PlayerPrefs.SetInt("soundOn", soundOn ? 1 : 0);
        }
        
        public void ToggleVibration()
        {
            vibrationOn = !vibrationOn;
            PlayerPrefs.SetInt("vibrationOn", vibrationOn ? 1 : 0);
        }
        
        public void ToggleNightMode(bool b = true)
        {
            nightModeOn = !nightModeOn;
            PlayerPrefs.SetInt("nightModeOn", nightModeOn ? 1 : 0);
        }

    #endregion
    
    [Button("Test")]
    public void showNightMode()
    {
        Debug.Log(Time.timeScale);
    }
    
    
}