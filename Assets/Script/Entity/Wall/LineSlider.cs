using System;
using DG.Tweening;
using UnityEngine;

public class LineSlider : MonoBehaviour
{
    public GameObject hoop;
    private void OnEnable()
    {
        if (Time.frameCount < 3) return;
        if (!transform.parent.name.Contains("Object Pooler"))
        {
            hoop = gameObject.transform.GetChild(0).gameObject;
            hoop.GetComponent<Hoop>().hoopInChallenge = true;
        }
        else
        {
            hoop = ObjectPooler.Instance.Spawn("Hoop");
            hoop.transform.SetParent(transform);
            hoop.transform.localPosition = Vector3.right * 1.5f + Vector3.up * 0.2f;
        }
        hoop.transform.DOLocalMoveX(-1.5f, 2.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            hoop.transform.DOLocalMoveX(1.5f, 2.5f).SetEase(Ease.InOutSine);
        }).SetLoops(-1, LoopType.Yoyo);
    }

    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
    }

    private void OnHoopPassed()
    {
        if (hoop != null && hoop.GetComponent<Hoop>().containsBall)
        {
            hoop.transform.DOKill();
            hoop.transform.SetParent(hoop.GetComponent<Hoop>().hoopInChallenge ? transform.parent : null);
            foreach (Transform t in transform)
            {
                t.GetComponent<Disappearance>().Excute();
            }
        }
    }
}
