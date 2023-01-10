using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class CamController : MonoBehaviour
{
    public GameObject ball;
    public Transform cam;
    public CinemachineVirtualCamera vcam;
    
    public CinemachineFramingTransposer transposer; 
    private Vector3 pivotPos;
    private bool locked;
    public GameObject bait;
    
    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
        this.RegisterListener(EventID.RestartLevel, (param) => OnRestartLevel());

        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        
        ball = GameObject.FindGameObjectWithTag("Ball");
        locked = false;
        
        ball.SetActive(false);
    }

    private void OnRestartLevel()
    {
        MoveCameraToOriginal();
    }

    private void LateUpdate()
    {
        if (ball.transform.position.y < pivotPos.y - GameManager.Instance.VerticalScreen / 2f - 0.5f)
        {
            if (ScoreManager.Score == 0)
            {
                GameManager.Instance.Continue();
            }
            else
            {
                if (GameManager.Instance.InChallenge &&
                    (LevelManager.Instance.currentLv.type.Equals("NewBall") |
                     LevelManager.Instance.currentLv.type.Equals("NoAim")) && LevelManager.Instance.hp > 0)
                {
                    GameManager.Instance.Continue();
                    LevelManager.Instance.hp--;
                    if (LevelManager.Instance.hp > 0)
                    {
                        return;
                    }
                }
                vcam.Follow = null;
                if (!locked)
                {
                    AfterGameOver();
                    locked = true;
                }
            }
        }
    }

    private void OnHoopPassed()
    {
        vcam.Follow = ball.transform;
        transposer.m_YDamping = 2f;
        pivotPos = transform.position;
        transposer.m_DeadZoneHeight = .1f;
        locked = false;
    }

    public void MoveCameraToOriginal()
    {
        vcam.Follow = bait.transform;
        transposer.m_YDamping = 0;
        //transposer.m_DeadZoneHeight = .3f;
        pivotPos = Vector3.up * -10f;
    }
    
    private async UniTaskVoid AfterGameOver()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        this.PostEvent(EventID.GameOver);
    }

     
}
