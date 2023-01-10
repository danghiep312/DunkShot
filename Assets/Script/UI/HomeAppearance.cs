using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeAppearance : MonoBehaviour
{
    public RectTransform[] fadeInObjects;
    public Button[] buttons;
    public RectTransform logo;
    public RectTransform freegift;
    public RectTransform customize;
    public RectTransform challenges;

    public void Awake()
    {
        Fadein(2f);
        freegift.localScale = customize.localScale = challenges.localScale = Vector3.zero;
        logo.anchoredPosition = Vector2.zero;
        logo.localScale = Vector3.zero;
        logo.DOScale(Vector3.one * 0.6f, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            logo.DOLocalMoveY(450, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                freegift.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);
                customize.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack).SetDelay(.4f);
                challenges.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack).SetDelay(.7f);
                GameManager.Instance.ball.gameObject.SetActive(true);
            });
        });

    }

    private void Fadein(float delayTime)
    {
        foreach (var item in buttons) item.GetComponent<Button>().interactable = false;
        foreach (var item in fadeInObjects)
        {
            if (item.GetComponent<TextMeshProUGUI>() != null)
            {
                item.GetComponent<TextMeshProUGUI>().DOFade(0, 0);
            }

            if (item.GetComponent<Image>() != null)
            {
                item.GetComponent<Image>().DOFade(0, 0);
            }
        }
        
        foreach (var item in fadeInObjects)
        { 
            if (item.GetComponent<TextMeshProUGUI>() != null)
            {
                item.GetComponent<TextMeshProUGUI>().DOFade(1, .5f).SetDelay(delayTime).OnComplete(() =>
                {
                    foreach (var item in buttons) item.GetComponent<Button>().interactable = true;
                });
            }
            else if (item.GetComponent<Image>() != null)
            {
                item.GetComponent<Image>().DOFade(1, .5f).SetDelay(delayTime);
            }
        }
        
    }
}