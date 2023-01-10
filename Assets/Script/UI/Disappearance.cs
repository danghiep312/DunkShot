using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Object = System.Object;

public class Disappearance : MonoBehaviour
{
    public float duration;
    public RectTransform rt;
    public Ease ease;
    

    public void Excute()
    {
        rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.DOScale(Vector3.zero, duration).SetEase(ease).SetUpdate(true);
        }
        else
        {
            transform.DOScale(Vector3.zero, duration).SetEase(ease).SetUpdate(true).OnComplete(() =>
            {
                if (gameObject.tag.Contains("Wall"))
                {
                    ObjectPooler.Instance.ReleaseObject(gameObject);
                }

                if (gameObject.name.Contains("Point"))
                {
                    ObjectPooler.Instance.ReleaseObject(transform.parent.gameObject);
                }
            });
        }
    }
}   
