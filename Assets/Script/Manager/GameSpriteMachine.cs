using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GameSpriteMachine : Singleton<GameSpriteMachine>
{
    public Skin skin;
    public Theme theme;
    
    public SpriteRenderer[] accessorySprites;
    public Image background;

    public Skin[] skins;
    public Theme[] themes;

    public override void Awake()
    {
        base.Awake();
        skins = Resources.LoadAll<Skin>("Skins");
        themes = Resources.LoadAll<Theme>("Themes");
    }

    private void Start()
    {
        this.RegisterListener(EventID.ThemeClick, (param) => OnThemeClick((int)param));
        skin = FindSkinById(PlayerPrefs.GetInt("SkinID", 100));
        theme = FindThemeById(PlayerPrefs.GetInt("ThemeID", 10));
    }

    private void OnThemeClick(int id)
    {
        theme = FindThemeById(id);
    }

    private void Update()
    {
        if (GameManager.Instance.nightModeOn)
        {
            background.sprite = theme.nightBackground;
            foreach (var a in accessorySprites)
            {
                a.sprite = theme.nightOffsetBackground;
            }
        }
        else
        {
            background.sprite = theme.lightBackground;
            foreach (var a in accessorySprites)
            {
                a.sprite = theme.lightOffsetBackground;
            }
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
