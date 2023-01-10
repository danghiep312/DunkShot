using TMPro;
using UnityEngine;

public class BestScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    public class Score : MonoBehaviour
    {
        public TextMeshProUGUI text;
    
        private void Update()
        {
            text.text = "<cspace=-1.5>BEST SCORE\n<size=135>" + ScoreManager.BestScore;
        }
    }
}