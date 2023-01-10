using System;
using DG.Tweening;
using UnityEngine;
public class GameOverPanel : MonoBehaviour
{
    public RectTransform[] rt;

    private void OnEnable()
    {
        for (int i = 0; i < rt.Length; i++)
        {
            rt[i].localScale = Vector3.zero;
            rt[i].DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCirc).SetDelay(i * 0.1f);
        }
    }
}