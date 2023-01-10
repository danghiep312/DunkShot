using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelNoti : MonoBehaviour
{
    public Image zone;
    public Image playButton;
    public Image rewardIcon;
    
    #region Sprite
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/New Ball")]
    public Sprite newBall;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/New Ball")]
    public Sprite newBallButton;
    
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Collect")]
    public Sprite collect;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Collect")]
    public Sprite collectButton;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Time")]
    public Sprite time;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Time")]
    public Sprite timeButton;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Score")]
    public Sprite score;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Score")]
    public Sprite scoreButton;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Bounce")]
    public Sprite bounce;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Bounce")]
    public Sprite bounceButton;
    
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/No Aim")]
    public Sprite noAim;
    [PreviewField, HideLabel, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/No Aim")]
    public Sprite noAimButton;

    [PreviewField, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Reward")]
    public Sprite ballLocked;

    [PreviewField, FoldoutGroup("Sprite and Color"), BoxGroup("Sprite and Color/Reward")]
    public Sprite token;
    #endregion
    
    private void OnEnable()
    {
        zone.sprite = LevelManager.Instance.currentLv.type switch
        {
            "NewBall" => newBall,
            "Collect" => collect,
            "Time" => time,
            "Score" => score,
            "Bounce" => bounce,
            "NoAim" => noAim,
            _ => null
        };
        
        playButton.sprite = LevelManager.Instance.currentLv.type switch
        {
            "NewBall" => newBallButton,
            "Collect" => collectButton,
            "Time" => timeButton,
            "Score" => scoreButton,
            "Bounce" => bounceButton,
            "NoAim" => noAimButton,
            _ => null
        };

        rewardIcon.sprite = LevelManager.Instance.currentLv.type switch
        {
            "NewBall" => ballLocked,
            _ => token
        };
    }
}
