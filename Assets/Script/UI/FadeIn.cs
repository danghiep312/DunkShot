using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public float fadeValue;
    public float fadeTime;
    public float delayTime;
    private void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        if (image != null)
        {
            image.DOFade(0, 0);
            image.DOFade(fadeValue, fadeTime).SetDelay(delayTime);
        }
        
        if (text != null)
        {
            text.DOFade(0, 0);
            text.DOFade(fadeValue, fadeTime).SetDelay(delayTime);
        }
        
    }
}
