using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPopup : MonoBehaviour
{
    public Image ballImage;
    public TextMeshProUGUI missionText;
    public Image fill;
    public TextMeshProUGUI progress;

    public Skin s;
    public void SetMission(Skin skin)
    {
        s = skin;
        ballImage.sprite = skin.ballSprite;
        missionText.text = skin.description;

        if (fill != null)
        {
            int currentValue = AchievementManager.Instance.GetCurrentValueProgress(skin);
            if (Regex.IsMatch(s.id.ToString(), SkinButton.VIDEO_PATTERN))
            {
                currentValue = PlayerPrefs.GetInt("Watch" + s.id);
            }
            fill.fillAmount = currentValue * 1f / skin.price;
        }
    }

    private void Update()
    {
        if (!Regex.IsMatch(s.id.ToString(), SkinButton.VIDEO_PATTERN)) return;
        var currentValue = PlayerPrefs.GetInt("Watch" + s.id);
        progress.text = currentValue + "/" + s.price;
    }

    public void WatchVideo()
    {
        PlayerPrefs.SetInt("Watch" + s.id, PlayerPrefs.GetInt("Watch" + s.id) + 1);
        this.PostEvent(EventID.WatchVideo);
    }
}
