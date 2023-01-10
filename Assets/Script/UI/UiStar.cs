using System;
using TMPro;
using UnityEngine;


public class UiStar : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        text.text = ScoreManager.Star.ToString();
    }
}