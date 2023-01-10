using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueImage : MonoBehaviour
{
    public Image fill;
    public float timeRemain;
    public float timeTotal;
    public Button skipButton;
    
    private void OnEnable()
    {
        timeRemain = timeTotal;
    }

    private void Update()
    {
        skipButton.gameObject.SetActive(timeRemain < timeTotal - 2f);
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
        }
        else
        {
            skipButton.onClick.Invoke();
        }

        fill.fillAmount = timeRemain / timeTotal;
    }
}
