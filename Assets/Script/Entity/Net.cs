using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour
{
    public Collider2D col;
    

    public void NonLaunch()
    {
        try
        {
            transform.DOScaleY(0.65f, 0.5f).SetEase(Ease.OutElastic);
        }
        catch (Exception e)
        {
            Common.LogWarning(this, e.Message);
        }
    }
    
    public void BallInHoop()
    {
        if (transform.parent.gameObject.activeSelf && !transform.parent.name.Contains("Finish") && GetComponentInParent<Hoop>().containsBall)
        {
            transform.DOScaleY(0.75f, 0.05f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                transform.DOScaleY(0.65f, 0.1f).SetEase(Ease.InQuad);
            });
        }
    }
    
    public void Launch()
    {
        try
        {
            transform.DOScaleY(0.65f, 0.5f).SetEase(Ease.OutElastic);
        }
        catch (Exception e)
        {
            Common.LogWarning(this, e.Message);
        }
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
