using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ThemeButton : MonoBehaviour
{
    public static string UNLOCKED_THEME = @"^.{1}0$";
    public static string NORMAL_PATTERN = @"^1.{1}$";
    public static string SEASON_PATTERN = @"^2.{1}$";

    public Theme theme;
    public Image icon;

    public GameObject selected;
    public GameObject locked;
    public TextMeshProUGUI price;

    public bool isUnlocked;
    public bool[] type = new[] { false, false }; // 0 is normal, 1 is season

    private void OnEnable()
    {
        try
        {
            isUnlocked = PlayerPrefs.GetInt(theme.id.ToString(), 0) == 1;
            if (!isUnlocked)
            {
                isUnlocked = Regex.IsMatch(theme.id.ToString(), UNLOCKED_THEME);
            }
        }
        catch (Exception e)
        {
            Common.LogWarning(this, e.Message);
        }
    }

    public void SetTheme(Theme t)
    {
        theme = t;
        icon.sprite = t.themeIcon;
        price.text = t.price.ToString();
        if (Regex.IsMatch(t.id.ToString(), NORMAL_PATTERN)) type[0] = true;
        if (Regex.IsMatch(t.id.ToString(), SEASON_PATTERN)) type[1] = true;
    }

    public void ButtonPressed()
    {
        if (isUnlocked)
        {
            this.PostEvent(EventID.ThemeClick, theme.id);
        }
        else
        {
            if (ScoreManager.Token >= theme.price)
            {
                ScoreManager.AddToken(-theme.price);
                isUnlocked = true;
                PlayerPrefs.SetInt(theme.id.ToString(), 1);
                ButtonPressed();
            }
        }
    }
    
    public void ButtonPressedWhenIsSelected()
    {
        if (!selected.activeSelf) return;
        PagesManager.Instance.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        locked.SetActive(!isUnlocked);
        price.transform.parent.gameObject.SetActive(!isUnlocked);
        selected.SetActive(PagesManager.Instance.themeID == theme.id);
    }
}