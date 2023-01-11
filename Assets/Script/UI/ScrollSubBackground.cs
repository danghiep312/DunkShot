using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ScrollSubBackground : MonoBehaviour
{
    [SerializeField] List<Transform> starsDecor;
    float foregroundDistance, starsDistance, velocity;
    [SerializeField] new Transform camera;
    [SerializeField] Vector3 lastPos;
    private void Awake()
    {
        starsDistance = starsDecor[1].transform.position.y - starsDecor[0].transform.position.y;
        
    }
    
    void FixedUpdate()
    {
        velocity = (camera.position.y - lastPos.y) / Time.fixedDeltaTime;
        lastPos = camera.position;
        
        for (int i = 0; i < starsDecor.Count; i++)
        {
            starsDecor[i].position = Vector3.Lerp(starsDecor[i].position, starsDecor[i].position - Vector3.up * velocity * 0.15f, Time.fixedDeltaTime);
            if (camera.position.y >= starsDecor[i].position.y)
            {
                starsDecor[1 - i].position = starsDecor[i].position + Vector3.up * starsDistance;
            }
            else
                starsDecor[1 - i].position = starsDecor[i].position - Vector3.up * starsDistance;
        }
    }
}
