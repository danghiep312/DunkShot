using TMPro;
using UnityEngine;


public class BounceHud : MonoBehaviour
{
    public TextMeshProUGUI titleLevel;
    public TextMeshProUGUI hoopRemaining;

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
        score.text = LevelManager.Instance.totalBounce + "/" + LevelManager.Instance.currentLv.target;
        hoopRemaining.text = LevelManager.Instance.hoopPassed + "/" + totalHoop + " HOOPS";
    }
    
}