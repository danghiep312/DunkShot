using System;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteNoti : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI reward;

    public Image button;

    public Image rewardIcon;

    private void OnEnable()
    {
        SetData(LevelManager.Instance.currentLv);
        
        ScoreManager.AddToken(LevelManager.Instance.currentLv.reward);
    }

    #region Sprite and Color
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/New Ball")]
    public Sprite newBall;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/New Ball")]
    public Color newBallColor;
    
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Collect")]
    public Sprite collect;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Collect")]
    public Color collectColor;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Time")]
    public Sprite time;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Time")]
    public Color timeColor;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Score")]
    public Sprite score;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Score")]
    public Color scoreColor;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Bounce")]
    public Sprite bounce;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Bounce")]
    public Color bounceColor;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/No Aim")]
    public Sprite noAim;
    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/No Aim")]
    public Color noAimColor;

    [HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Icon")]
    public Sprite token;
    #endregion
    
    
    public void SetData(Level lv)
    {
        title.text = "CHALLENGE " + int.Parse(lv.id.ToString().Substring(1, 2)) + " COMPLETE!";
        reward.text = "+" + lv.reward;
        if (lv.type.Equals("NewBall")) reward.text = "";
        button.sprite = lv.type switch
        {
            "NewBall" => newBall,
            "Collect" => collect,
            "Time" => time,
            "Score" => score,
            "Bounce" => bounce,
            "NoAim" => noAim,
            _ => button.sprite
        };
        description.color = lv.type switch
        {
            "NewBall" => newBallColor,
            "Collect" => collectColor,
            "Time" => timeColor,
            "Score" => scoreColor,
            "Bounce" => bounceColor,
            "NoAim" => noAimColor,
            _ => description.color
        };

        rewardIcon.sprite = lv.type switch
        {
            "NewBall" => GameSpriteMachine.Instance.FindSkinById(int.Parse("4" + lv.id.ToString().Substring(1, 2))).ballSprite,
            _ => token
        };
        rewardIcon.GetComponent<RectTransform>().anchoredPosition = lv.type switch
        {
            "NewBall" => Vector2.up * -300f,
            _ => Vector2.up * -260f
        };
    }
    
    
}
