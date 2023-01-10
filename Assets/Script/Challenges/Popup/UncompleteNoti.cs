using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UncompleteNoti : MonoBehaviour
{
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

    #endregion
    
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public Image restartButton;

    public void SetData(Level lv, int hoopPassed, bool isPause)
    {
        title.text = isPause ? "PAUSED" 
            : "CHALLENGE " + int.Parse(lv.id.ToString().Substring(1, 2)) + " FAILED";
        description.text = lv.type switch
        {
            "NewBall" => "Hoops completed " + LevelManager.Instance.hoopPassed + "/" + LevelManager.Instance.currentLv.target,
            "Collect" => LevelManager.Instance.totalToken + "/" + LevelManager.Instance.currentLv.target + " token, " +
                         (lv.totalHoop - hoopPassed) + " hoops left",
            "Time" => "Hoops completed " + LevelManager.Instance.hoopPassed + "/" + LevelManager.Instance.currentLv.target,
            "Score" => ScoreManager.Score + "/" + LevelManager.Instance.currentLv.target + " score, " +
                       (lv.totalHoop - hoopPassed) + " hoops left",
            "Bounce" => LevelManager.Instance.totalBounce + "/" + LevelManager.Instance.currentLv.target + " bounces, " +
                        (lv.totalHoop - hoopPassed) + " hoops left",
            "NoAim" => "Hoops completed " + LevelManager.Instance.hoopPassed + "/" + LevelManager.Instance.currentLv.target,
            _ => ""
        };
        
        restartButton.sprite = lv.type switch
        {
            "NewBall" => newBall,
            "Collect" => collect,
            "Time" => time,
            "Score" => score,
            "Bounce" => bounce,
            "NoAim" => noAim,
            _ => restartButton.sprite
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
    }
}
