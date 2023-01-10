using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class TransitionWithChallenge : MonoBehaviour
{
    public Image image;
    
    private void Start()
    {
        image = GetComponent<Image>();
        this.RegisterListener(EventID.GoToChallenge, (param) => OnGoToChallenge());
        this.RegisterListener(EventID.BackFromChallenge, (param) => BackFromChallenge());
    }

    private void OnGoToChallenge()
    {
        image.DOFade(0, 0.1f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (gameObject.name.Equals("Bg")) transform.parent.gameObject.SetActive(false);
        });
    }


    private void BackFromChallenge()
    {
        if (gameObject.name.Equals("Bg")) transform.parent.gameObject.SetActive(true);
        image.DOFade(1, 0.1f).SetUpdate(true).SetEase(Ease.Linear);
    }
}
