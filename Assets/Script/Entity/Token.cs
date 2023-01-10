using System;
using DG.Tweening;
using UnityEngine;

public class Token : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject ring;

    public bool inChallenge;
    private void OnEnable() 
    {
        sr.enabled = true;
        if (inChallenge)
        {
            sr.transform.DOLocalMoveY(0.3f, 0.7f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                sr.transform.DOLocalMoveY(-0.3f, 0.7f).SetEase(Ease.InOutSine);
            }).SetLoops(-1, LoopType.Yoyo); 
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            sr.enabled = false;
            ring.SetActive(true);
            ring.transform.localScale = Vector3.one * .1f;
            ring.transform.DOScale(Vector3.one * .2f, .2f);
            ring.GetComponent<SpriteRenderer>().DOFade(0f, 0.2f).OnComplete(Reset);
            this.PostEvent(EventID.GetToken);
        }
    }

    public void Reset()
    {
        sr.enabled = true;
        ring.SetActive(true);
        ring.transform.localScale = Vector3.one * .1f;
        ring.GetComponent<SpriteRenderer>().DOFade(1f, 0f);
        ObjectPooler.Instance.ReleaseObject(gameObject);
    }
}
