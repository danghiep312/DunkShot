using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class ScoreHud : MonoBehaviour
{
    public TextMeshProUGUI hoopRemaining;
    public TextMeshProUGUI titleLevel;

    public int totalHoop;
    public TextMeshProUGUI score;

    
    
    private void OnEnable()
    {
        totalHoop = LevelManager.Instance.currentLv.totalHoop;
        score.text = "0/" + LevelManager.Instance.currentLv.target;
    }
    

    private void Update()
    {
        titleLevel.text = "CHALLENGE " + int.Parse(LevelManager.Instance.currentLv.id.ToString().Substring(1, 2));
        score.text = ScoreManager.Score + "/" + LevelManager.Instance.currentLv.target;
        hoopRemaining.text = LevelManager.Instance.hoopPassed + "/" + totalHoop + " HOOPS";
    }
}
