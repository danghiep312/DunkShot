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
    private SpriteRenderer[] pointSprites;
    
    [SerializeField]
    [Range(1, 20)]
    private int numberOfPoints;

    private void Start()
    {
        this.RegisterListener(EventID.Launch, param => OnLaunchBall());

        points = new GameObject[numberOfPoints];
        pointSprites = new SpriteRenderer[points.Length];
        //spaceBetweenPoints = new float[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, transform);
            pointSprites[i] = points[i].GetComponent<SpriteRenderer>();
            if (i > 0)
            {
                points[i].transform.localScale = points[i - 1].transform.localScale * 0.98f;
            }
        }

        SetVisibleOfTrajectoryLine(false);
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
    
    public void SetAlphaOfTrajectoryLine(float alpha)
    {
        for (var i = 0; i < numberOfPoints; i++)
        {
            var color = pointSprites[i].color;
            color.a = alpha;
            pointSprites[i].color = color;
        }
    }
}
