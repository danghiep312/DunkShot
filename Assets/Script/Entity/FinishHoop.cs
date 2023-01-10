using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class FinishHoop : MonoBehaviour
{
    public Transform target;
    
    [Header("State of hoop")]
    public bool containsBall;
    public bool isPassed;
    public bool touchHoop;      

    [Header("Object of hoop")]
    public GameObject powerRing;
    public GameObject[] accessories;
    public GameObject beams;

    public void OnEnable()
    {
        isPassed = false;
        containsBall = false;
        touchHoop = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        beams.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        containsBall = true;
        other.transform.SetParent(transform);
        if (other.CompareTag("Ball") && !other.GetComponent<Ball>().inHoop)
        {
            other.transform.SetParent(transform);
            
            if (!touchHoop && !isPassed)
            {
                this.PostEvent(EventID.ReachPerfect);
            }
            
            if (!isPassed && !gameObject.name.Contains("First"))
            {
                var score = GameManager.Instance.GetStreak();
                if (GameManager.Instance.GetBounce() > 0) score *= 2;
                NotiScore.Instance.ScoredNotification(GameManager.Instance.GetStreak() > 1, GameManager.Instance.GetBounce(), score);
            }

            if (!isPassed)
            {
                Array.ForEach(accessories, go => go.SetActive(false));
                if (GameManager.Instance.GetStreak() > 1)
                {
                    DoPerfectPowerRingEffect(); 
                }
                else
                {
                    DoNormalPowerRingEffect();                    
                }
                this.PostEvent(EventID.PassHoopChallenge);
            }
            
            this.PostEvent(EventID.HoopPassed, isPassed);
            this.PostEvent(EventID.ReachFinishHoop);
            beams.SetActive(true);
            Debug.Log("Finish Challenge");
            isPassed = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            touchHoop = true;
        }
    }
    
    [Button("Normal effect")]
    public void DoNormalPowerRingEffect()
    {
        powerRing.transform.localScale = Vector3.one * 0.4f;
        powerRing.GetComponent<SpriteRenderer>().DOFade(1, 0f);
        powerRing.SetActive(true);
        powerRing.transform.DOScale(Vector3.one * 0.65f, 0.5f).OnComplete(() =>
        {
            powerRing.SetActive(false);
        });
        powerRing.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
    }
    
    [Button("Perfect effect")]
    public void DoPerfectPowerRingEffect()
    {
        powerRing.transform.localScale = Vector3.one * 0.4f;
        powerRing.GetComponent<SpriteRenderer>().DOFade(1, 0f);
        powerRing.SetActive(true);
        powerRing.transform.DOScale(Vector3.one * 0.9f, 0.5f).OnComplete(() =>
        {
            powerRing.SetActive(false);
        });
        powerRing.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
    }
}
