using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour
{
    public Collider2D col;
    
    private void Start()
    {
        this.RegisterListener(EventID.Launch, (param) => OnLaunch());
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
    }

    private void OnHoopPassed()
    {
        if (GetComponentInParent<Hoop>().containsBall)
        {
            transform.DOScaleY(0.75f, 0.05f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                transform.DOScaleY(0.65f, 0.1f).SetEase(Ease.InQuad);
            });
        }
    }
    
    private void OnLaunch()
    {
        transform.DOScaleY(0.65f, 0.5f).SetEase(Ease.OutElastic);
    }

    public void SetStateCollider(bool status)
    {
        col.enabled = status;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            transform.DOScaleY(0.6f, 0.05f).OnComplete(() =>
            {
                transform.DOScaleY(0.65f, 0.05f);
            });
        }
    }
}
