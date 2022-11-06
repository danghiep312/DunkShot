using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopSpawner : Singleton<HoopSpawner>
{
    public GameObject firstHoop;
    public GameObject currentHoop;
    public GameObject nextHoop;

    public void Start()
    {
        currentHoop = firstHoop;
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
        nextHoop = ObjectPooler.Instance.Spawn("Hoop");
        nextHoop.transform.position = Vector3.right * 2.2f + Vector3.up * 1f;
    }

    
    
    public void SpawnNewWave()
    {
        currentHoop = nextHoop;
        nextHoop = ObjectPooler.Instance.Spawn("Hoop");
        nextHoop.transform.position = Vector3.left * currentHoop.transform.position.x + Vector3.up * 1f;
    }
    
    private void OnHoopPassed()
    {
        
    }
}
