using System;
using UnityEngine;

public class BallTrail : MonoBehaviour
{
    public ParticleSystem smoke;
    public ParticleSystem smokeFlame;
    public ParticleSystem flame;

    private void Start()
    {
        this.RegisterListener(EventID.SkinClick, (param) => OnSkinClick((int)param));
        OnSkinClick(PlayerPrefs.GetInt("BallID", 100));
    }
    
    private void OnSkinClick(int id)
    { 
        var s = GameSpriteMachine.Instance.FindSkinById(id);
        var smokeFlameColor = smokeFlame.colorOverLifetime;
        smokeFlameColor.color = s.smokeColor;
        var flameColor = flame.colorOverLifetime;
        flameColor.color = s.colorOfFlame;
    }
    
    public void PlayParticleSystem(int streak)
    {
        if (streak >= 4)
        {
            smoke.Stop();
            smokeFlame.Play();
            flame.Play();
        }   
        else if (streak == 3)
        {
            smoke.Play();
        }
        else
        {
            smoke.Stop();
            smokeFlame.Stop();
            flame.Stop();
        }
    }
}
