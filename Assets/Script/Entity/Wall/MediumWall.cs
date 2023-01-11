using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MediumWall : MonoBehaviour
{
    public GameObject hoop;

    private void OnEnable()
    {
        if (Time.frameCount < 3) return;
        if (!transform.parent.name.Contains("Object Pooler"))
        {
            hoop = gameObject.transform.GetChild(0).gameObject;
            hoop.GetComponent<Hoop>().hoopInChallenge = true;
            return;
        }
        hoop = ObjectPooler.Instance.Spawn("Hoop");
        hoop.transform.SetParent(transform);
        hoop.GetComponent<Hoop>().Appearance(Vector3.right * 2.5f + Vector3.up * 2f);
        //hoop.transform.position = transform.position + Vector3.up * -1.5f
        //                               + Vector3.right * 3.5f * HoopSpawner.Instance.currentZone * -1f;
        hoop.transform.localPosition = Vector3.right * 3.5f * HoopSpawner.Instance.currentZone * -1f 
                                       + Vector3.up * -1.5f;
    }

    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
    }

    private void OnHoopPassed()
    {
        if (hoop.GetComponent<Hoop>().containsBall)
        {
            hoop.transform.SetParent(null);
            GetComponent<Disappearance>().Excute();
        }
    }
    
}
