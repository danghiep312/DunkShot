using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiToken : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        text.text = ScoreManager.Token.ToString();
    }
}
