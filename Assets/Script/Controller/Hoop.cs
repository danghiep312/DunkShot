using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public Transform belowPoint;
    public Transform abovePoint;
    private Vector2 allowDir;
    public bool containsBall;
    public Transform anchorPos;
    public Transform bottomPos;
    public Transform net;
    
    
    private void Start()
    {
        this.RegisterListener(EventID.Drag, (param) => OnDrag((Vector2)param));
        this.RegisterListener(EventID.Launch, (param) => OnLaunchBall());
    }

    private void Update()
    {
        allowDir = abovePoint.position - belowPoint.position;
        if (containsBall)
        {
            anchorPos.localPosition = bottomPos.localPosition + Vector3.up * (float) (0.8 * 0.65 / net.localScale.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Vector2 _velocity = other.GetComponent<Rigidbody2D>().velocity;
            //Common.Log(Vector2.Angle(allowDir, _velocity));
            if (Vector2.Angle(allowDir, _velocity) < 90f)
            {
                containsBall = true;
                other.transform.SetParent(transform);
                net.GetComponent<Net>().SetStateCollider(false);
                ResetRotation();
                this.PostEvent(EventID.HoopPassed, anchorPos);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Vector2 _velocity = other.GetComponent<Rigidbody2D>().velocity;
            //Common.Log(Vector2.Angle(allowDir, _velocity));
            if (Vector2.Angle(allowDir, _velocity) > 90f)
            {
                net.GetComponent<Net>().SetStateCollider(true);
            }
        }
    }

    private void OnDrag(Vector2 velocity)
    {
        if (!containsBall) return;
        net.localScale = Vector3.one * 0.65f + Vector3.up * (float) (velocity.magnitude * (1.2 - 0.65f) / 25);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f - Vector2.SignedAngle(velocity, Vector2.right)));
    }

    private void OnLaunchBall()
    {
        containsBall = false;
    }

    public void ResetRotation()
    {
        transform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.InOutQuart);
    }
}
