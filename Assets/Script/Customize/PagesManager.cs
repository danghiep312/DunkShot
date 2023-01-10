using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PagesManager : Singleton<PagesManager>
{
    
    public int ballID;
    public int themeID;
    public GameObject ballButtonPrefab;
    public GameObject themeButtonPrefab;
    
    [Header("Data")]
    // Data to init
    public Skin[] skins;
    public SkinButton[] skinButtons;
    public Theme[] themes;
    public ThemeButton[] themeButtons;

    [Header("Ball Zone")]
    // Hold zone of ball in shop
    public ContentSizeFitter ballContentSizeFitter;
    
    public GameObject normalZone;
    public GridLayoutGroup normalZoneGrid;
    
    public GameObject missionZone;
    public GridLayoutGroup missionZoneGrid;

    public GameObject videoZone;
    public GridLayoutGroup videoZoneGrid;

    public GameObject challengeZone;
    public GridLayoutGroup challengeZoneGrid;
    
    [Header("Theme Zone")]
    // Hold zone of theme in shop
    public GameObject classicZone;
    public GridLayoutGroup classicZoneGrid;
    public GameObject seasonsZone;
    public GridLayoutGroup seasonsZoneGrid;
    
    [Header("Hint Panel")]
    public GameObject hintPanel;
    public GameObject starPanel;
    public GameObject missionBallPanel;
    public GameObject videoBallPanel;
    public GameObject challengeBallPanel;

    [Header("Handle Tab Button")]
    // Handle tab button
    public Image ballButton;
    public Image themeButton;
    public Color defaultColor;

    [Header("Star and Token")]
    public GameObject star;
    public GameObject token;
    
    
    public override void Awake()
    {
        base.Awake();
        skins = Resources.LoadAll<Skin>("Skins");
        skinButtons = new SkinButton[skins.Length];
        themes = Resources.LoadAll<Theme>("Themes");
        themeButtons = new ThemeButton[themes.Length];
    }

    private void OnEnable()
    {
        ClickBallTab();
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        this.RegisterListener(EventID.SkinClick, (param) => OnSkinClick((int)param));
        this.RegisterListener(EventID.ThemeClick, (param) => OnThemeClick((int)param));

        ballID = PlayerPrefs.GetInt("BallID", 100);
        themeID = PlayerPrefs.GetInt("ThemeID", 10);
        
        int[] numOfBall = new int[] {0, 0, 0, 0};
        int[] numOfTheme = new int[] {0, 0};
        for (var i = 0; i < skins.Length; i++)
        {
            GameObject g = null;
            if (Regex.IsMatch(skins[i].id.ToString(), SkinButton.NORMAL_PATTERN))
            {
                g = Instantiate(ballButtonPrefab, normalZoneGrid.transform);
                numOfBall[0]++;
            }
            else if (Regex.IsMatch(skins[i].id.ToString(), SkinButton.VIDEO_PATTERN))
            {
                g = Instantiate(ballButtonPrefab, videoZoneGrid.transform);
                numOfBall[1]++;
            }
            else if (Regex.IsMatch(skins[i].id.ToString(), SkinButton.MISSION_PATTERN))
            {
                g = Instantiate(ballButtonPrefab, missionZoneGrid.transform);
                numOfBall[2]++;
            }
            else if (Regex.IsMatch(skins[i].id.ToString(), SkinButton.CHALLENGE_PATTERN))
            {
                g = Instantiate(ballButtonPrefab, challengeZoneGrid.transform);
                numOfBall[3]++;
            }
            
            skinButtons[i] = g.GetComponent<SkinButton>();
            skinButtons[i].SetSkin(skins[i]);
            g.transform.localScale = Vector3.one;
        }

        for (var i = 0; i < themes.Length; i++)
        {
            GameObject g = null;
            if (Regex.IsMatch(themes[i].id.ToString(), ThemeButton.NORMAL_PATTERN))
            {
                g = Instantiate(themeButtonPrefab, classicZoneGrid.transform);
                numOfTheme[0]++;
            }
            else if (Regex.IsMatch(themes[i].id.ToString(), ThemeButton.SEASON_PATTERN))
            {
                g = Instantiate(themeButtonPrefab, seasonsZoneGrid.transform);
                numOfTheme[1]++;
            }

            
            themeButtons[i] = g.GetComponent<ThemeButton>();
            themeButtons[i].SetTheme(themes[i]);
            g.transform.localScale = Vector3.one;
       
        }
        
        missionZoneGrid.cellSize = normalZoneGrid.cellSize = videoZoneGrid.cellSize = challengeZoneGrid.cellSize
            = Vector2.one * CalculateCellSizeBallTab();
        classicZoneGrid.cellSize =
            seasonsZoneGrid.cellSize = Vector2.right * CalculateCellSizeThemeTab() + Vector2.up * 360f;

        ballContentSizeFitter.enabled = false;
        ballContentSizeFitter.enabled = true;
        CalculateHeightZone(normalZone, normalZoneGrid, numOfBall[0]);
        CalculateHeightZone(videoZone, videoZoneGrid, numOfBall[1]);
        CalculateHeightZone(missionZone, missionZoneGrid, numOfBall[2]);
        CalculateHeightZone(challengeZone, challengeZoneGrid, numOfBall[3]);
        
        CalculateHeightZone(classicZone, classicZoneGrid, numOfTheme[0]);
        CalculateHeightZone(seasonsZone, seasonsZoneGrid, numOfTheme[1]);
        ClickBallTab();

        transform.parent.gameObject.SetActive(false);
    }

    private void OnThemeClick(int id)
    {
        themeID = id;
        Save();
    }

    private void OnSkinClick(int id)
    {
        ballID = id;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("BallID", ballID);
        PlayerPrefs.SetInt("ThemeID", themeID);
    }

    public void ShowStarPanel()
    {
        hintPanel.SetActive(true);
        starPanel.SetActive(true);
    }
    
    public void ShowMissionBallPanel()
    {
        hintPanel.SetActive(true);
        missionBallPanel.SetActive(true);
    }

    public void ShowVideoBallPanel()
    {
        hintPanel.SetActive(true);
        videoBallPanel.SetActive(true);
    }

    public void ShowChallengeBallPanel()
    {
        hintPanel.SetActive(true);
        challengeBallPanel.SetActive(true);
    }
    
    public void ClickThemeTab()
    {
        normalZone.transform.parent.parent.parent.gameObject.SetActive(false);
        classicZone.transform.parent.parent.parent.gameObject.SetActive(true);
        themeButton.color = defaultColor;
        ballButton.color = Color.grey;
        
        star.SetActive(false);
        token.SetActive(true);
    }

    public void ClickBallTab()
    {
        classicZone.transform.parent.parent.parent.gameObject.SetActive(false);
        normalZone.transform.parent.parent.parent.gameObject.SetActive(true);
        ballButton.color = defaultColor;
        themeButton.color = Color.grey;
        
        star.SetActive(true);
        token.SetActive(false);
    }
    
    private float CalculateCellSizeBallTab()
    {
        return (normalZone.GetComponent<RectTransform>().rect.width -
                (normalZoneGrid.constraintCount - 1) * normalZoneGrid.spacing.x 
                - normalZoneGrid.padding.left 
                - normalZoneGrid.padding.right) / normalZoneGrid.constraintCount;
    }

    private float CalculateCellSizeThemeTab()
    {
        return normalZone.GetComponent<RectTransform>().rect.width;
    }

    private void CalculateHeightZone(GameObject zone, GridLayoutGroup container, int coefficient)
    {
        var rect = zone.GetComponent<RectTransform>().rect;
        rect.height = container.spacing.y * (coefficient - 1) + (container.cellSize.y * coefficient / container.constraintCount) + container.padding.top + container.padding.bottom;
        zone.GetComponent<RectTransform>().sizeDelta = rect.size;
    }

    
}
