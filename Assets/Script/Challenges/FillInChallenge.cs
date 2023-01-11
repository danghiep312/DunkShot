using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillInChallenge : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI progress;
    
    private void OnEnable()
    {
        var mode = transform.parent.parent.name;
        var type = mode switch
        {
            "NewBall" => 1,
            "Collect" => 2,
            "Time" => 3, 
            "Score" => 4,
            "Bounce" => 5,
            "NoAim" => 6,
            _ => 0
        };
        fill.fillAmount = (float) LevelManager.Instance.GetNumberCompleteLevel(type) /
                          LevelManager.Instance.GetNumberLevelOfType(type);
        progress.text = LevelManager.Instance.GetNumberCompleteLevel(type) * 100 /
                        LevelManager.Instance.GetNumberLevelOfType(type) + "%";
    }
}
