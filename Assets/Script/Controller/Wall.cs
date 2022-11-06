using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform[] walls;
    public Transform cam;
    private void Start()
    {
        foreach (var w in walls)
        {
            w.localScale = new Vector3(1, GameManager.Instance.VerticalScreen, 1);
            if (w.name.Contains("Left"))
            {
                w.position = Vector3.left * (GameManager.Instance.HorizontalScreen / 2 + 0.5f) + Vector3.up * cam.position.y;
            }
            else
            {
                w.position = Vector3.right * (GameManager.Instance.HorizontalScreen / 2 + 0.5f) + Vector3.up * cam.position.y;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var w in walls)
        {
            w.position = new Vector3(w.position.x, cam.position.y, w.position.y);
        }
    }
}
