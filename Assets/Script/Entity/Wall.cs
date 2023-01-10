using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject[] fakeWalls;
    public Transform[] walls;
    public Transform cam;
    private void Start()
    {
        foreach (var w in walls)
        {
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
        if (fakeWalls.Length == 0)
        {
            fakeWalls = GameObject.FindGameObjectsWithTag("FakeObstacle");
        }
        foreach (var w in walls)
        {
            var position = w.position;
            position = new Vector3(position.x, cam.position.y, position.y);
            w.position = position;
        }

        foreach (var w in fakeWalls)
        {
            var position = w.transform.position;
            position = new Vector3(position.x, cam.position.y, position.y);
            w.transform.position = position;
        }
    }
}
