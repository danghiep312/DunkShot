using System;
using DG.Tweening;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float duration;
    private void OnEnable()
    {
        transform.DORotate(Vector3.forward * 180f, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DORotate(Vector3.forward * 360f, duration).SetEase(Ease.Linear);
        }).SetLoops(-1);
    }

    
}