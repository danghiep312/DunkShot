using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject ball;
    public Transform cam;
    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        this.RegisterListener(EventID.Drag, (param) => OnDrag());
    }

    private void OnDrag()
    {
        cam.position = new Vector3(cam.position.x, ball.transform.position.y, cam.position.z);
    }
}
