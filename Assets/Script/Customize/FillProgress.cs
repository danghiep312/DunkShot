using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FillProgress : MonoBehaviour
{
    public Image fill;
    public Skin skin;

    public TextMeshProUGUI progress;
    private void Start()
    {
        skin = GetComponentInParent<SkinButton>().skin;
    }

    private void Update()
    {
        int currentValue = AchievementManager.Instance.GetCurrentValueProgress(skin);
        if (Regex.IsMatch(skin.id.ToString(), SkinButton.VIDEO_PATTERN))
        {
            currentValue = PlayerPrefs.GetInt("Watch" + skin.id);
        }

        if (progress != null) progress.text = (skin.price - currentValue).ToString();
        fill.fillAmount = currentValue * 1f / skin.price;
    }
}