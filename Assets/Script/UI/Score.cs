using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    private void Update()
    {
        text.text = ScoreManager.Score.ToString();
    }
    
}
