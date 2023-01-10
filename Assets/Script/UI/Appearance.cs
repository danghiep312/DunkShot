using System;
using DG.Tweening;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    public float duration;
    public float delay;
    public RectTransform rt;
    public Ease ease;
    private void OnEnable()
    {
        rt = GetComponent < RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.zero;
            rt.DOScale(Vector3.one, duration).SetEase(ease).SetDelay(delay).SetUpdate(true);
        }
        else
        {
            transform.localScale = Vector3.zero;
            if (gameObject.name.Equals("beams"))
            {
                transform.DOScale(Vector3.one * 0.5f, duration).SetEase(ease).SetDelay(delay).SetUpdate(true);
                return;
            }
            Vector3 target = gameObject.name.Contains("Point")
                ? Vector3.one * .18f
                : Vector3.right * .4f + Vector3.up * .5f;
            transform.DOScale(target, duration).SetEase(ease).SetDelay(delay).SetUpdate(true);
        }
    }
}