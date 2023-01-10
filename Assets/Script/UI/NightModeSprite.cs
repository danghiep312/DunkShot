using System;
using UnityEngine;
using UnityEngine.UI;


public class NightModeSprite : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public Image image;
    
    private void Update()
    {
        image.sprite = GameManager.Instance.nightModeOn ? offSprite : onSprite;
    }
}
