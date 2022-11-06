using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject BallPrefab;
    
    public GameObject Ball;
    public float HorizontalScreen;
    public float VerticalScreen;
    
    public override void Awake()
    {
        base.Awake();
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(Vector2.one);
        HorizontalScreen = edgeVector.x * 2;
        VerticalScreen = edgeVector.y * 2;
        // Spawn ball when game starts
        Ball = ObjectPooler.Instance.Spawn("Ball");
        Ball.transform.position = Vector3.right * -2.2f + Vector3.up * -1.215f;
    }

    
    private void Start()
    {
        PredictionManager.Instance.CopyAllObstacles();
    }
}
