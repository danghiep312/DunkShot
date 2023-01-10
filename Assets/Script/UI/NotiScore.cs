using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotiScore : Singleton<NotiScore>
{
    public RectTransform holder;
    
    public RectTransform perfect;
    public RectTransform bounce;
    public RectTransform score;

    public Vector3 currentHoop;
    public GameObject ball;

    private void Update()
    {
        if (ball == null)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
        }
        else
        {
            if (ball.transform.parent != null)
            {
                currentHoop = ball.transform.parent.position;
            }
        }

        holder.position = currentHoop;
    }
    
    public void ScoredNotification(bool reachPerfect, int reachBounce, int scored)
    {
        if (reachBounce == 1 && !GameManager.Instance.InChallenge)
        {
            bounce.GetComponent<TextMeshProUGUI>().text = "BOUNCE";
        }
        else
        {
            bounce.GetComponent<TextMeshProUGUI>().text = "BOUNCE x" + reachBounce;
        }
        score.GetComponent<TextMeshProUGUI>().text = "+" + scored;
        perfect.anchoredPosition = bounce.anchoredPosition = score.anchoredPosition = new Vector2(0, 0);
        if (reachPerfect)
        {
            perfect.DOLocalMoveY(150, .3f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() =>
            {
                perfect.DOLocalMoveY(175, .3f).SetEase(Ease.InQuad).SetUpdate(true).SetDelay(.2f);
            });
            perfect.GetComponent<TextMeshProUGUI>().DOFade(1f, .3f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                perfect.GetComponent<TextMeshProUGUI>().DOFade(0f, .3f).SetUpdate(true).SetDelay(.2f);
            });
        }

        if (reachBounce > 0 && !GameManager.Instance.InChallenge)
        {
            bounce.DOLocalMoveY(125, .3f).SetEase(Ease.OutQuad).SetDelay(.2f).SetUpdate(true).OnComplete(() =>
            {
                bounce.DOLocalMoveY(150, .3f).SetEase(Ease.InQuad).SetDelay(.2f).SetUpdate(true);
            });
        
            bounce.GetComponent<TextMeshProUGUI>().DOFade(1f, .3f).SetEase(Ease.OutQuad).SetDelay(.2f).SetUpdate(true).OnComplete(() =>
            {
                bounce.GetComponent<TextMeshProUGUI>().DOFade(0f, .3f).SetDelay(.2f).SetUpdate(true);
            });
        }
        
        this.PostEvent(EventID.AddScore, scored);
        if (GameManager.Instance.InChallenge) return;
        score.DOLocalMoveY(75, .5f).SetEase(Ease.OutQuad).SetDelay(.4f).SetUpdate(true).OnComplete(() =>
        {
            score.DOLocalMoveY(100, .3f).SetEase(Ease.InQuad).SetDelay(.3f).SetUpdate(true);
        });
        
        score.GetComponent<TextMeshProUGUI>().DOFade(1f, .5f).SetEase(Ease.OutQuad).SetDelay(.4f).SetUpdate(true).OnComplete(() =>
        {
            score.GetComponent<TextMeshProUGUI>().DOFade(0f, .3f).SetDelay(.3f).SetUpdate(true);
        });
    }
    
}
