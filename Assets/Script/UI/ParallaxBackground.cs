using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cam;
    public Transform[] accessories;
    public bool isFollowingCam;
    public GameObject lowestBg;
    public Transform bait;
    public float size;

    private void Start()
    {
        this.RegisterListener(EventID.GoHome, (param) => GoHome());
        size = accessories[0].GetComponent<SpriteRenderer>().bounds.size.y;
        accessories[1].position = Vector3.zero;
        accessories[0].position = accessories[1].position - Vector3.up * size;
        accessories[2].position = accessories[1].position + Vector3.up * size;
    }

    private void GoHome()
    {
        accessories[1].position = bait.position + Vector3.right * -bait.position.y;
        accessories[0].position = accessories[1].position - Vector3.up * size;
        accessories[2].position = accessories[1].position + Vector3.up * size;
        
        
    }

    private void Update()
    {
        lowestBg = GetLowestBg();
        if (lowestBg.transform.position.y + lowestBg.GetComponent<SpriteRenderer>().bounds.extents.y <
            cam.position.y - GameManager.Instance.VerticalScreen / 2)
        {
            lowestBg.transform.position = GetHighestBg().transform.position + Vector3.up * size;
        }
    }

    private GameObject GetHighestBg()
    {
        GameObject highestBg = null;
        foreach (var ac in accessories)
        {
            if (highestBg == null)
            {
                highestBg = ac.gameObject;
            }
            else
            {
                if (ac.position.y > highestBg.transform.position.y)
                {
                    highestBg = ac.gameObject;
                }
            }
        }

        return highestBg;
    }

    private GameObject GetLowestBg()
    {
        GameObject lowestBg = null;
        foreach (var ac in accessories)
        {
            if (lowestBg == null)
            {
                lowestBg = ac.gameObject;
            }
            else
            {
                if (ac.position.y < lowestBg.transform.position.y)
                {
                    lowestBg = ac.gameObject;
                }
            }
        }

        return lowestBg;
    }
}