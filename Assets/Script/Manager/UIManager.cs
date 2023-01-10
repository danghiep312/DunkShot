using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject GameplayPanel;
    public GameObject[] ChallengePanels;
    public GameObject GameOverPanel;
    public GameObject ContinuePanel;
    public GameObject HomePanel;
    public GameObject PausePanel;
    public GameObject LevelNoti;
    public GameObject CompleteChallengePanel;
    public GameObject UncompleteChallengePanel;
    public GameObject PauseInChallengePanel;
    public GameObject[] Popups;
    
    public Image transition;
    private void Start()
    {
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        this.RegisterListener(EventID.PlayGame, (param) => OnPlayGame());
        this.RegisterListener(EventID.GoToChallenge, (param) => GoToChallenge((string)param));
        this.RegisterListener(EventID.CompleteChallenge, (param) => OnCompleteChallenge());
        this.RegisterListener(EventID.UncompleteChallenge, (param) => OnUncompleteChallenge());
  
        GameOverPanel.SetActive(false);
    }

    private void OnUncompleteChallenge()
    {
        ShowUncompleteChallengePanel();
    }

    private void OnCompleteChallenge()
    {
        CompleteChallengePanel.SetActive(true);
    }

    private void OnPlayGame()
    {
        HomePanel.SetActive(false);
        GameplayPanel.SetActive(true);
    }

    private void OnGameOver()
    {
        if (!GameManager.Instance.InChallenge)
        {
            if (GameManager.Instance.canContinue)
            {
                ContinuePanel.SetActive(true);
            }
            else
            {
                GameOverPanel.SetActive(true);
            }
        }
    }

    public void Continue()
    {
        GameManager.Instance.Continue();
        ContinuePanel.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void GoHome()
    {
        transition.raycastTarget = true;
        transition.DOFade(1f, 0.1f).OnComplete(() =>
        {
            HomePanel.SetActive(true);
            GameplayPanel.SetActive(false);
            PausePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            transition.DOFade(0, 0.1f).OnComplete(() =>
            {
                transition.raycastTarget = false;
            });
        });
        Array.ForEach(Popups, p => p.SetActive(false));
    }

    public void GoToChallenge(string n)
    {
        GameplayPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        PausePanel.SetActive(false);
        HomePanel.SetActive(false);
        ContinuePanel.SetActive(false);
        Array.ForEach(ChallengePanels, p => p.SetActive(p.name.Contains(n)));
        
        transition.raycastTarget = false;
        ShowLevelNotiPanel(Resources.Load<Level>("Levels/" + LevelManager.Instance.currentLv.id));
    }

    public void ShowLevelNotiPanel(Level lv)
    {
        LevelNoti.SetActive(true);
        LevelNoti.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "CHALLENGE " + Convert.ToInt32(lv.id.ToString().Substring(1));
        LevelNoti.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = lv.description;
    }

    public void BackFromChallenge()
    {
        GameManager.Instance.GoHome();
        Array.ForEach(ChallengePanels, p => p.SetActive(false));
        LevelNoti.SetActive(false);
        this.PostEvent(EventID.BackFromChallenge);
    }

    public void OkWhenFinishChallenge()
    {
        GameManager.Instance.GoHome();
        Array.ForEach(ChallengePanels, p => p.SetActive(false));
        LevelNoti.SetActive(false);
    }
    
    public void PauseInChallenge() {
        PauseInChallengePanel.SetActive(true);
        PauseInChallengePanel.GetComponent<UncompleteNoti>().SetData(LevelManager.Instance.currentLv, LevelManager.Instance.hoopPassed, true);
        Time.timeScale = 0;
    }
    
    public void ShowUncompleteChallengePanel() {
        UncompleteChallengePanel.SetActive(true);
        UncompleteChallengePanel.GetComponent<UncompleteNoti>().SetData(LevelManager.Instance.currentLv, LevelManager.Instance.hoopPassed, false);
    }

    public GameObject GetHudChallenge(string type)
    {
        return Array.Find(ChallengePanels, h => h.name.Contains(type));
    }
    
    public void ResetStatusTimeHud()
    {
        GetHudChallenge("Time").GetComponent<TimeHud>().ResetStatus();
    }

    public void RestartLevel()
    {
        transition.raycastTarget = true;
        transition.DOFade(0.7f, 0.05f).SetUpdate(true).OnComplete(() =>
        {
            LevelManager.Instance.RestartLevel();
            transition.DOFade(0, 0.05f).SetUpdate(true).OnComplete(() =>
            {
                transition.raycastTarget = false;
            });
        });
        Array.ForEach(Popups, p => p.SetActive(false));
    }
}