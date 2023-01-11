using System;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NewBallHud : MonoBehaviour
{
    public TextMeshProUGUI hoopRemaining;
    public TextMeshProUGUI titleLevel;

    public int totalHoop;
    public Image[] hp;

    private void OnEnable()
    {
        totalHoop = LevelManager.Instance.currentLv.totalHoop;
    }

    private void Start()
    {
        this.RegisterListener(EventID.PlayChallenge, (param) => OnPlayChallenge());
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
    }

    private void OnGameOver()
    {
        for (var i = hp.Length - 1; i >= 0; i--)
        {
            if (!hp[i].gameObject.activeSelf) continue;
            hp[i].gameObject.SetActive(false);
            return;
        }
    }

    public void Update()
    {
        titleLevel.text = "CHALLENGE " + int.Parse(LevelManager.Instance.currentLv.id.ToString().Substring(1, 2));
        hoopRemaining.text = LevelManager.Instance.hoopPassed + "/" + totalHoop + " HOOPS";
        for (var i = 0; i < hp.Length; i++)
        {
            hp[i].gameObject.SetActive(i < LevelManager.Instance.hp);
        }
    }

    public void OnPlayChallenge()
    {
        Array.ForEach(hp, i => i.gameObject.SetActive(true));
    }
    
    
}