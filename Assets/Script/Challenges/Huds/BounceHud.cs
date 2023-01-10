using TMPro;
using UnityEngine;


public class BounceHud : MonoBehaviour
{
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
        score.text = LevelManager.Instance.totalBounce + "/" + LevelManager.Instance.currentLv.target;
        hoopRemaining.text = LevelManager.Instance.hoopPassed + "/" + totalHoop + " HOOPS";
    }
    
}