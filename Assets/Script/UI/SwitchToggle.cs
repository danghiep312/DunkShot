using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SwitchToggle : MonoBehaviour
{
    //public int id; // 0 is sound 1 is vibration 2 is nightmode
    public RectTransform uiHandleRectTransform;
    public Color backgroundActiveColor;
    private Toggle toggle;

    private Image backgroundImage, handleImage;

    Color backgroundDefaultColor;

    private Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        //CheckStatusWhenStart();
        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        uiHandleRectTransform.anchoredPosition = toggle.isOn ? handlePosition * -1 : handlePosition;
        backgroundImage.color = toggle.isOn ? backgroundActiveColor : backgroundDefaultColor;
    }

    private void OnEnable()
    {
        CheckStatusWhenStart();
        if (!gameObject.name.Contains("NightMode")) return;
        toggle.onValueChanged.AddListener(GameManager.Instance.ToggleNightMode);
        //toggle.isOn = PlayerPrefs.GetInt("nightModeOn", 0) == 1;
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(GameManager.Instance.ToggleNightMode);
    }

    private void OnSwitch(bool on)
    {
        uiHandleRectTransform.DOAnchorPos (on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack).SetUpdate(true);
        uiHandleRectTransform.DOScaleY(.6f, .2f).SetUpdate(true).OnComplete(() =>
        {
            uiHandleRectTransform.DOScaleY(1f, .2f).SetUpdate(true);
        });
        backgroundImage.DOColor (on ? backgroundActiveColor : backgroundDefaultColor, .4f).SetUpdate(true);
    }

    private void CheckStatusWhenStart()
    {
        if (gameObject.name.Contains("Sound"))
        {
            toggle.isOn = PlayerPrefs.GetInt("soundOn", 1) == 1;
        }
        else if (gameObject.name.Contains("Vibration"))
        {
            toggle.isOn = PlayerPrefs.GetInt("vibrationOn", 1) == 1;
        }
        else if (gameObject.name.Contains("NightMode"))
        {
            toggle.isOn = PlayerPrefs.GetInt("nightModeOn", 0) == 1;
        }
    }
}