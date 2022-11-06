using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : Singleton<DrawTrajectory>
{

    public bool keepPredictLine;
    // predict line
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject[] points;

    [SerializeField]
    [Range(1, 20)]
    private int numberOfPoints;

    private void Start()
    {
        this.RegisterListener(EventID.Launch, param => OnLaunchBall());

        points = new GameObject[numberOfPoints];
        //spaceBetweenPoints = new float[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, transform);
        }

        SetVisibleOfTrajectoryLine(false);
    }

    public void UpdateTrajectoryLine(Vector3 force, Rigidbody2D rb, Vector3 startingPoint)
    {
        var velocity = (force / rb.mass) * Time.fixedDeltaTime;
        
        var timeToMaxHeight = velocity.y / Physics2D.gravity.y;
        for (int i = 1; i <= numberOfPoints; i++)
        {
            if (i <= timeToMaxHeight)
            {
                points[i - 1].transform.position = new Vector3(velocity.x * i, velocity.y * i, 0) + startingPoint;
            }
            else
            {
                points[i - 1].transform.position = new Vector3(velocity.x * i, velocity.y * timeToMaxHeight + Physics2D.gravity.y * (i - timeToMaxHeight) * (i - timeToMaxHeight) / 2, 0) + startingPoint;
            }
        }
    }

    public void SetPosOfPoint(Vector3[] pos, int step)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = pos[i * step + 3];
        }
    }

    public void SetVisibleOfTrajectoryLine(bool visible)
    {
        for (var i = 0; i < numberOfPoints; i++)
        {
            points[i].SetActive(visible);
        }
    }

    private void OnLaunchBall()
    {
        if (!keepPredictLine)
        {
            SetVisibleOfTrajectoryLine(false);
        }
    }
}
