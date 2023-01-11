using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameSpriteMachine : Singleton<GameSpriteMachine>
{
    public Skin skin;
    public Theme theme;
    
    public SpriteRenderer[] accessorySprites;
    public SpriteRenderer[] decorations;
    public Image background;

    public Skin[] skins;
    public Theme[] themes;

    [Space(30)] 
    [Header("Change color")]
    public Image[] buttonImage;
    public Image[] panels;
    public TextMeshProUGUI[] texts;
    public Image transition;
    public Color darkColor, lightColor;
    public TextMeshProUGUI score;
    public override void Awake()
    {
        base.Awake();
        skins = Resources.LoadAll<Skin>("Skins");
        themes = Resources.LoadAll<Theme>("Themes");
    }

    private void Start()
    {
        this.RegisterListener(EventID.ThemeClick, (param) => OnThemeClick((int)param));
        this.RegisterListener(EventID.ToggleNightMode, (param) => OnToggleNightMode());
        skin = FindSkinById(PlayerPrefs.GetInt("SkinID", 100));
        theme = FindThemeById(PlayerPrefs.GetInt("ThemeID", 10));
    }

    private void OnToggleNightMode()
    {
        transition.color = GameManager.Instance.nightModeOn ? darkColor : lightColor;
    }

    private void OnThemeClick(int id)
    {
        theme = FindThemeById(id);
        score.color = GameManager.Instance.nightModeOn ? theme.scoreDarkColor : theme.scoreColor;
        Array.ForEach(buttonImage, b => b.color = theme.buttonColor);
        Array.ForEach(texts, t => t.color = theme.buttonColor);
        Array.ForEach(panels, p => p.color = GameManager.Instance.nightModeOn ? darkColor : lightColor);
        
        if (GameManager.Instance.nightModeOn)
        {
            background.sprite = theme.nightBackground;
            Array.ForEach(accessorySprites, a => a.sprite = theme.nightOffsetBackground);
            Array.ForEach(decorations, d => d.sprite = theme.secondNightOffsetBackground);
        }
        else
        {
            background.sprite = theme.lightBackground;
            Array.ForEach(accessorySprites, a => a.sprite = theme.lightOffsetBackground);
        }
    }
    
    
    public Skin FindSkinById(int id)
    {
        return skins.FirstOrDefault(s => s.id == id);
    }

    public Theme FindThemeById(int id)
    {
        return themes.FirstOrDefault(t => t.id == id);
    }
    
    
}
