using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoopSpawner : Singleton<HoopSpawner>
{
    public GameObject firstHoop;
    public GameObject currentHoop;
    public GameObject nextHoop;
    
    public GameObject veryLargeWall;
    public GameObject largeWall;
    public GameObject mediumWall;
    public GameObject smallWall;
    public GameObject spin;
    public GameObject lineSlider;

    public int type;
    public int preZone; // -1 is left 1 is right
    public int currentZone;
    
    public float vertical, horizontal;
    public void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed((bool)param));
        this.RegisterListener(EventID.GameOver, (param) => OnGameOver());
        currentHoop = firstHoop;
        AfterStart();
        preZone = currentZone = 1;
        
        vertical = GameManager.Instance.VerticalScreen;
        horizontal = GameManager.Instance.HorizontalScreen;
    }

    public void CallStart()
    {
        firstHoop.SetActive(true);
        firstHoop.GetComponent<Hoop>().ResetHoop();
        firstHoop.GetComponent<Hoop>().isPassed = true;

        preZone = 1;
        currentHoop = firstHoop;
        nextHoop = ObjectPooler.Instance.Spawn("Hoop");
        nextHoop.transform.position = Vector3.right * 2.2f + Vector3.up * 1f;
    }
    
    public void SpawnNewWave()
    {
        //ObjectPooler.Instance.ReleaseObject(currentHoop);
        currentHoop = nextHoop;
        int preType = type;
        currentZone = preZone == 1 ? -1 : 1;
        preZone = currentZone;
        type = Random.Range(0, 5);
        
        switch (type)
        {
            case 0:
                nextHoop = ObjectPooler.Instance.Spawn("Hoop");
                nextHoop.transform.position = Vector3.right * Random.Range(2f, horizontal / 2 - 1.5f) * currentZone +
                                              (currentHoop.transform.position.y + Random.Range(2.2f, 3f)) * Vector3.up;
                break;
            case 1:
                nextHoop = ObjectPooler.Instance.Spawn("Hoop");
                nextHoop.transform.position = Vector3.right * Random.Range(2f, horizontal / 2 - 1.5f) * currentZone +
                                              (currentHoop.transform.position.y + Random.Range(2.2f, 3f)) * Vector3.up;
                nextHoop.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(15f, 30f) * currentZone);
                break;
            case 2:
                mediumWall = ObjectPooler.Instance.Spawn("MediumWall");
                mediumWall.transform.position = Vector3.right * (horizontal / 2 - Random.Range(.5f, 1f)) * currentZone +
                                                Vector3.up * (currentHoop.transform.position.y + 4f);
                nextHoop = mediumWall.GetComponent<MediumWall>().hoop;
                break;
            case 3:
                lineSlider = ObjectPooler.Instance.Spawn("LineSlider");
                lineSlider.transform.position = Vector3.right * Random.Range(-1f, 1f) + Vector3.up * (nextHoop.transform.position.y + 5f);
                nextHoop = lineSlider.GetComponent<LineSlider>().hoop;
                break;
            case 4:
                currentZone = preZone == 1 ? -1 : 1;
                preZone = currentZone;
                veryLargeWall = ObjectPooler.Instance.Spawn("VeryLargeWall");
                veryLargeWall.transform.position = Vector3.up * (currentHoop.transform.position.y + Random.Range(5f, 7f));
                nextHoop = veryLargeWall.GetComponent<VeryLargeWall>().hoop;
                break;
            case 5:
                // spin = ObjectPooler.Instance.Spawn("Spin");
                // nextHoop = ObjectPooler.Instance.Spawn("Hoop");
                // nextHoop.transform.position = Vector3.right * Random.Range(2f, horizontal / 2 - 1.5f) * currentZone +
                //                               (currentHoop.transform.position.y + Random.Range(2.2f, 3f)) * Vector3.up;
                // spin.transform.position = Vector3.up * (nextHoop.transform.position.y + currentHoop.transform.position.y) / 2;
                break;
            case 6:
                break;
            case 7: 
                break;
            case 8:
                
                break;
            case 9:
                break;
        }
        //Debug.Break();
    }
    
    private void OnHoopPassed(bool isPassed)
    {
        if (GameManager.Instance.InChallenge) return;
        
        if (!isPassed)
        {
            currentHoop.GetComponent<Hoop>().Disappear();
            SpawnNewWave();
        }
    }

    private void OnGameOver()
    {
        if (GameManager.Instance.InChallenge) return;
        if (currentHoop.transform.localScale == Vector3.one && !GameManager.Instance.canContinue)
        {
            currentHoop.GetComponent<Hoop>().Disappear();
        }
    }

    public GameObject GetCurrentHoop()
    {
        return currentHoop;
    }

    
    public bool CheckSpawnRule(int pre, int cur)
    {
        if (cur == 4 && pre >= 2) return false;
        return true;
    }
    
    private async UniTaskVoid AfterStart()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2.4f));
        nextHoop = ObjectPooler.Instance.Spawn("Hoop");
        nextHoop.transform.position = Vector3.right * 2.2f + Vector3.up * 1f;
    }
}
