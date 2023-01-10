using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public Transform belowPoint;
    public Transform abovePoint;
    private Vector2 allowDir;
    
    public Transform anchorPos;
    public Transform bottomPos;
    public Transform net;
    public Transform target;
    
    [Header("State of hoop")]
    public bool containsBall;
    public bool isPassed;
    public bool touchHoop;
    public bool hoopInChallenge;

    [Header("Object of hoop")]
    public GameObject powerRing;
    public SpriteRenderer backHoopSprite;
    public SpriteRenderer frontHoopSprite;
    public SpriteRenderer[] launchRings;

    // Set theme
    public Sprite backHoopSpriteTheme;
    public Sprite frontHoopSpriteTheme;
    public Sprite frontHoopInactiveSpriteTheme;
    public Sprite backHoopInactiveSpriteTheme;
    
    private void Start()
    {
        this.RegisterListener(EventID.ThemeClick, (param) => OnThemeClick((int)param));
        this.RegisterListener(EventID.Drag, (param) => OnDrag((Vector2)param));
        this.RegisterListener(EventID.Launch, (param) => OnLaunchBall());
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
        
        OnThemeClick(PlayerPrefs.GetInt("ThemeID", 10));
        if (gameObject.name.Contains("1stHoop"))
        {
            backHoopSprite.sprite = backHoopInactiveSpriteTheme;
            frontHoopSprite.sprite = frontHoopInactiveSpriteTheme;
        }
    }

    private void OnEnable()
    {
        //Common.LogWarning(this, "Appear");
        if (!gameObject.name.Contains("FirstHoop") &&  !gameObject.name.Contains("1stHoop")) ResetHoop();
        if (gameObject.name.Contains("FirstHoop") && Time.frameCount < 60)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack).SetDelay(2f);
            Debug.Log("FirstHoop");
        }
        else
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutExpo);
           
        }
    }

    public void Appearance(Vector3 tarScale)
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;
        transform.DOScale(tarScale, .4f).SetEase(Ease.OutExpo);
    }

    private void OnThemeClick(int id)
    {
        Theme t = GameSpriteMachine.Instance.FindThemeById(id);
        backHoopSpriteTheme = t.backHoop;
        frontHoopSpriteTheme = t.frontHoop;
        frontHoopInactiveSpriteTheme = t.frontHoopInactive;
        backHoopInactiveSpriteTheme = t.backHoopInactive;
        if (isPassed && !gameObject.name.Equals("FirstHoop"))
        {
            frontHoopSprite.sprite = frontHoopInactiveSpriteTheme;
            backHoopSprite.sprite = backHoopInactiveSpriteTheme;
        }
        else
        {
            frontHoopSprite.sprite = frontHoopSpriteTheme;
            backHoopSprite.sprite = backHoopSpriteTheme;
        }
    }
    
    public void Disappear()
    {
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            //Common.LogWarning(this, "Disappear");
            OnOffHoop(true);
            if (gameObject.name.Equals("FirstHoop"))
            {
                gameObject.SetActive(false);
            }
            else
            {
                ObjectPooler.Instance.ReleaseObject(gameObject);
            }
        });        
    }

    private void Update()
    {
        if (GameManager.Instance.InChallenge)
        {
            if (Camera.main.transform.position.y - 20f > transform.position.y)
            {
                gameObject.SetActive(false);
            }
        }
        allowDir = abovePoint.position - belowPoint.position;
        anchorPos.localPosition = bottomPos.localPosition + Vector3.up * (float) (0.8 * 0.65 / net.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Ball") && !other.GetComponent<Ball>().inHoop)
        {
            
            //Common.Log(Vector2.Angle(allowDir, _velocity));
            
            containsBall = true;
            other.transform.SetParent(transform);
           
            ResetRotation();
            //this.PostEvent(EventID.HoopPassed);
            
            if (!touchHoop && !isPassed)
            {
                // reach perfect
                this.PostEvent(EventID.ReachPerfect);
            }
            if (!isPassed && !gameObject.name.Contains("First"))
            {
                OnOffHoop(false);

                var score = GameManager.Instance.GetStreak();
                if (GameManager.Instance.GetBounce() > 0) score *= 2;
                NotiScore.Instance.ScoredNotification(GameManager.Instance.GetStreak() > 1, GameManager.Instance.GetBounce(), score);
            }

            if (!isPassed)
            {
                if (GameManager.Instance.GetStreak() > 1)
                {
                    DoPerfectPowerRingEffect();
                }
                else
                {
                    DoNormalPowerRingEffect();                    
                }

                if (hoopInChallenge)
                {
                    this.PostEvent(EventID.PassHoopChallenge);
                }
            }
            
            this.PostEvent(EventID.HoopPassed, isPassed);
            isPassed = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector2 _velocity = other.GetComponent<Rigidbody2D>().velocity;
            //Common.Log(Vector2.Angle(allowDir, _velocity));
            if (Vector2.Angle(allowDir, _velocity) > 90f)
            {
                other.GetComponent<Ball>().inHoop = false;
                net.GetComponent<Net>().SetStateCollider(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            touchHoop = true;
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
        ResetHoop();
    }

    private void OnHoopPassed()
    {
        if (containsBall) HoopSpawner.Instance.currentHoop = gameObject;
        touchHoop = false;
    }

    private void OnDrag(Vector2 velocity)
    {
        if (!containsBall) return;
        net.localScale = Vector3.one * 0.65f + Vector3.up * (float) (velocity.magnitude * (1.2 - 0.65f) / 25);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f - Vector2.SignedAngle(velocity, Vector2.right)));
    }

    private void OnLaunchBall()
    {
        
        try
        {
            if (!gameObject.activeSelf) return;
            if (containsBall)
            {
                DoLaunchRingEffect();
            }
            containsBall = false;
            if (GameManager.Instance.InChallenge) ResetRotation();
        }
        catch (Exception e)
        {
            Common.LogWarning(this, "OnLaunchBall", e);
        }
        
    }

    public void ResetRotation()
    {
        transform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.InOutQuart);
    }

    public void ResetHoop()
    {
        isPassed = false;
        containsBall = false;
        touchHoop = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        OnOffHoop(true);
    }
    
    public void OnOffHoop(bool state) // true is active, false is inactive
    {
        frontHoopSprite.sprite = state ? frontHoopSpriteTheme : frontHoopInactiveSpriteTheme;
        backHoopSprite.sprite = state ? backHoopSpriteTheme : backHoopInactiveSpriteTheme;
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
    
    [Button("Launch ring effect")]
    public void DoLaunchRingEffect()
    {
        for (int i = 0; i < launchRings.Length; i++)
        {
            launchRings[i].transform.localScale = Vector3.one * 0.3f;
            launchRings[i].DOFade(0, 0);
            launchRings[i].transform.DOScale(Vector3.zero, 0.4f).SetDelay(0.1f * (i + 1));
            launchRings[i].DOFade(1, 0.2f).SetDelay(0.1f * (i + 1));
            launchRings[i].DOFade(0, 0.2f).SetDelay(0.1f * (i + 1) + 0.2f);
        }
    }
    

    
}
