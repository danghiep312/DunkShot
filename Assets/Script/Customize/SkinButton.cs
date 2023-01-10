using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    public static string UNLOCKED_PATTERN = @"^.{1}00$";
    public static string NORMAL_PATTERN = @"^1.{2}$";
    public static string VIDEO_PATTERN = @"^2.{2}$";
    public static string MISSION_PATTERN = @"^3.{2}$";
    public static string CHALLENGE_PATTERN = @"^4.{2}$";

    public Skin skin;
    public Image ballImage;
    public TextMeshProUGUI price;

    public GameObject star;
    public GameObject selected;
    public GameObject unselected;
    public GameObject info;
    public GameObject infoVideo;
    public bool isUnlocked;
    public bool[] type = new [] {false, false, false, false}; // 0 is normal ball, 1 is video ball, 2 is mission ball, 3 is challenge ball

    private void OnEnable()
    {
        try
        {
            
            isUnlocked = PlayerPrefs.GetInt(skin.id.ToString(), 0) == 1;
            if (!isUnlocked)
            {
                isUnlocked = Regex.IsMatch(skin.id.ToString(), UNLOCKED_PATTERN);
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public void SetSkin(Skin s)
    {
        skin = s;
        ballImage.sprite = skin.ballSprite;
        if (Regex.IsMatch(s.id.ToString(), NORMAL_PATTERN))
        {
            star.SetActive(true);
            price.text = s.price.ToString();
        }
        
        isUnlocked = PlayerPrefs.GetInt(s.id.ToString(), 0) == 1;
        if (!isUnlocked)
        {
            isUnlocked = Regex.IsMatch(skin.id.ToString(), UNLOCKED_PATTERN);
        }
        if (Regex.IsMatch(skin.id.ToString(), NORMAL_PATTERN)) type[0] = true;
        if (Regex.IsMatch(skin.id.ToString(), VIDEO_PATTERN)) type[1] = true;
        if (Regex.IsMatch(skin.id.ToString(), MISSION_PATTERN)) type[2] = true;
        if (Regex.IsMatch(skin.id.ToString(), CHALLENGE_PATTERN)) type[3] = true;
    }

    private void Update()
    {
        if (!isUnlocked)
        {
            isUnlocked = PlayerPrefs.GetInt(skin.id.ToString(), 0) == 1;
        }
        selected.SetActive(skin.id == PagesManager.Instance.ballID);
        if (type[2]) 
        {
            unselected.SetActive(!isUnlocked);
            info.SetActive(!isUnlocked);
        }
        else
        {
            ballImage.sprite = !isUnlocked ? skin.offsetSprite : skin.ballSprite;
            if (type[0])
            {
                star.SetActive(!isUnlocked);
            }

            if (type[1])
            {
                infoVideo.SetActive(!isUnlocked);
            }
            
        }
        
    }
    
    public void ButtonPressedWhenIsSelected()
    {
        if (!selected.activeSelf) return;
        PagesManager.Instance.transform.parent.gameObject.SetActive(false);
    }

    public void ButtonPressed()
    {
        if (isUnlocked)
        {
            this.PostEvent(EventID.SkinClick, skin.id);
        }
        else
        {
            if (type[0])
            {
                if (ScoreManager.Star >= skin.price)
                {
                    isUnlocked = true;
                    PlayerPrefs.SetInt(skin.id.ToString(), 1);
                    ScoreManager.AddStar(-skin.price);
                    this.PostEvent(EventID.SkinClick, skin.id);
                }
                else PagesManager.Instance.ShowStarPanel();
            }
            else if (type[2])
            {
                if (!isUnlocked)
                {
                    PagesManager.Instance.ShowMissionBallPanel();
                    PagesManager.Instance.missionBallPanel.GetComponent<MissionPopup>().SetMission(skin);
                }
            }
            else if (type[1])
            {
                if (!isUnlocked)
                {
                    PagesManager.Instance.ShowVideoBallPanel();
                    PagesManager.Instance.videoBallPanel.GetComponent<MissionPopup>().SetMission(skin);
                }
            }
            else if (type[3])
            {
                if (!isUnlocked)
                {
                    PagesManager.Instance.ShowChallengeBallPanel();
                    PagesManager.Instance.challengeBallPanel.GetComponent<MissionPopup>().SetMission(skin);
                }
            }
        }
    }

}
