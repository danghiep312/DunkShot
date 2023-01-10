using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public int Streak = 1;
    public int Bounce = 0;
    
    public Transform anchorPosInHoop;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float power; 
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 _velocity;

    public SpriteRenderer srBall;
    public bool isAiming;
    public bool inHoop;
    public bool passHoop;
    public Vector3 target;

    public ParticleSystem smoke;
    public ParticleSystem[] flame;

    public bool canShoot;
    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
        this.RegisterListener(EventID.ReachPerfect, (param) => OnReachPerfect());
        this.RegisterListener(EventID.SkinClick, (param) => OnSkinClick((int)param));
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        this.RegisterListener(EventID.GoHome, (param) => OnGoHome());
        this.RegisterListener(EventID.PlayChallenge, (param) => OnPlayChallenge());
        this.RegisterListener(EventID.Launch, (param) => OnLaunch());
        this.RegisterListener(EventID.TimeOut, (param) => OnTimeOut());
        
        srBall.sprite = GameSpriteMachine.Instance.FindSkinById(PlayerPrefs.GetInt("BallID", 100)).ballSprite;
        canShoot = true;
    }

    private void OnTimeOut()
    {
        canShoot = false;
    }
    
    private void OnLaunch()
    {
        Bounce = 0;
    }

    private void OnPlayChallenge()
    {
        Streak = 1;
        canShoot = true;
    }

    public void ResetStatus()
    {
        Streak = 1;
        Bounce = 0;
        isAiming = false;
        inHoop = false;
        passHoop = false;
        canShoot = true;
        SetBallStatus(false);
        SetBallStatus(true);
    }

    private void Update()
    {

        if (inHoop)
        {
            transform.position = anchorPosInHoop.position;
        }

        if (passHoop)
        {
            rb.simulated = false;
            transform.DOMove(target, 0.05f).OnComplete(() =>
            {
                passHoop = false;
                inHoop = true;
                GetComponentInParent<Hoop>().net.GetComponent<Net>().BallInHoop();
            });
        }
        
        if (Input.GetMouseButtonDown(0) && !GameManager.IsMouseOverUI() && inHoop)
        {
            if (transform.parent != null && transform.parent.gameObject.name.Contains("FinishHoop")) return;
            if (GameManager.Instance.InHome)
            {
                if (GameManager.Instance.InChallenge == false)
                {
                    this.PostEvent(EventID.PlayGame);
                }
                GameManager.Instance.InHome = false;
            }
            startPosition = GetMousePosition();
            isAiming = true;
        }

        if (isAiming)
        {
            endPosition = GetMousePosition();
            _velocity = (startPosition - endPosition) * power;
            
            if (_velocity.magnitude > 30f)
            {
                _velocity = _velocity.normalized * 30f;
            }
            
            float alpha = (_velocity.magnitude - 10) / 10f;
            DrawTrajectory.Instance.SetAlphaOfTrajectoryLine(alpha);
            
            if (_velocity.magnitude < 10f)
            {
                DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(false);
            }
            else
            {
                DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(true);
                DrawTrajectory.Instance.SetPosOfPoint(
                    PredictionManager.Instance.Predict(transform.position, _velocity), 6);
                if (GameManager.Instance.InChallenge && LevelManager.Instance.currentLv != null && LevelManager.Instance.currentLv.type.Equals("NoAim"))
                {
                    DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(false);
                }
            }
            
            //Common.Log(_velocity.magnitude, this);
            if (_velocity.magnitude > 0)
            {
                this.PostEvent(EventID.Drag, _velocity);
            }
        }

        if (Input.GetMouseButtonUp(0) && inHoop && isAiming)
        {
            
            startPosition = endPosition = Vector2.zero;
            isAiming = false;
            if (_velocity.magnitude > 10f && canShoot)
            {
                rb.simulated = true;
                rb.angularVelocity = Random.Range(50f, 1200f);
                rb.velocity = _velocity;
                GetComponentInParent<Hoop>().net.GetComponent<Net>().Launch();
                transform.SetParent(null);
                SetBallStatus(true);
                //this.PostEvent(EventID.Launch);
                this.PostEvent(EventID.Launch);
                if (GameManager.Instance.InChallenge)
                {
                    this.PostEvent(EventID.StartChallenge);
                }
            }
            else
            {
                DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(false);
                GetComponentInParent<Hoop>().net.GetComponent<Net>().NonLaunch();
                this.PostEvent(EventID.NonLaunch);
            }

            _velocity = Vector2.zero;
        }
        
    }

    public void ResetStreak()
    {
        Bounce = 0;
        Streak = 1;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Hoop"))
        {
            Streak = 1;
            PlayParticleSystem(Streak);
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            Bounce++;
        }
    }
    
    private void OnSkinClick(int id)
    {
        srBall.sprite = GameSpriteMachine.Instance.FindSkinById(id).ballSprite;
    }

    private void OnGoHome()
    {
        canShoot = true;
        inHoop = false;
        target = Vector3.zero;
        gameObject.SetActive(true);
        transform.position = Vector3.right * -2.2f + Vector3.up * 1f;
        SetBallStatus(false);
        SetBallStatus(true);
    }
    
    private void OnGameOver()
    {
        ResetStreak();
        PlayParticleSystem(Streak);
    }
    
    private void OnHoopPassed()
    {
        passHoop = true;
        anchorPosInHoop = transform.parent.GetChild(2).GetChild(1);
        try
        {
            target = transform.parent.GetComponent<Hoop>().target.position;
        }
        catch (NullReferenceException e)
        {
            target = transform.parent.GetComponent<FinishHoop>().target.position;
        }
        
        SetBallStatus(false);
    }
    
    private void OnReachPerfect()
    {
        Streak++;
        PlayParticleSystem(Streak);
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    public void SetBallStatus(bool tr)
    {
        // tr = true is launch, tr = false is hoop passed
        //col.enabled = tr;
        if (tr)
        {
            rb.simulated = true;
        }
        else
        {
            //inHoop = true;
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void PlayParticleSystem(int streak)
    {
        if (streak >= 4)
        {
            smoke.Stop();
            foreach (var ps in flame)
            {
                ps.Play();
            }      
        }   
        else if (streak == 3)
        {
            smoke.Play();
        }
        else
        {
            smoke.Stop();
            foreach (var ps in flame)
            {
                ps.Stop();
            } 
        }
    }

    public void Spawn(Vector3 pos)
    {
        transform.position = pos;
        SetBallStatus(false);
        SetBallStatus(true);
        inHoop = false;
    }
    
    public void SetSprite(Sprite sprite)
    {
        srBall.sprite = sprite;
    }
    
    [Button("Show")]
    public void Show()
    {
        Debug.Log(Bounce + " " + Streak);
    }
    
    
}
