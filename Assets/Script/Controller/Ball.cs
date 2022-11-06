using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public Transform anchorPosInHoop;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float power; 
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 _velocity;
    private bool inHoop;
    public Collider2D col;
    
    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPass((Transform)param));
    }

    private void Update()
    {
        if (inHoop)
        {
            transform.position = anchorPosInHoop.position;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = GetMousePosition();
            DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(true);
        }

        if (Input.GetMouseButton(0))
        {
            DrawTrajectory.Instance.SetVisibleOfTrajectoryLine(true);
            endPosition = GetMousePosition();
            _velocity = (startPosition - endPosition) * power;
            
            if (_velocity.magnitude > 25f)
            {
                _velocity = _velocity.normalized * 25f;
            }

            if (_velocity.magnitude < 5f)
            {
                _velocity = Vector2.zero;
            }
            this.PostEvent(EventID.Drag, _velocity);
         
            DrawTrajectory.Instance.SetPosOfPoint(
                PredictionManager.Instance.Predict(GameManager.Instance.BallPrefab, transform.position, _velocity), 3);
        }

        if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = _velocity;
            transform.SetParent(null);
            SetBallStatus(true);
            this.PostEvent(EventID.Launch);
        }
        
    }
    
    private void OnHoopPass(Transform point)
    {
        anchorPosInHoop = point;
        SetBallStatus(false);
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetBallStatus(bool tr)
    {
        // tr = true is launch, tr = false is hoop passed
        if (tr)
        {
            inHoop = false;
            col.enabled = true;
            rb.simulated = true;
        }
        else
        {
            inHoop = true;
            col.enabled = false;
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
