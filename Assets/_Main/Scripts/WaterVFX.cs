using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class WaterVFX : HoseParticle
{
    public float originSpeed, originLT;
    ParticleSystem.MainModule main;
    public float curSpeed, curLifeTime;
    private void Start()
    {
        if(vfx ==null)
            vfx = transform.GetComponentInChildren<ParticleSystem>(true);
        main = vfx.main;
        originSpeed = main.startSpeed.constant;
        originLT = main.startLifetime.constant;

        EventManager.StartListening(EventKey.onEffectIntensityChanged.ToString(), OnEffectIntensityChanged);
    }
    public override void HandleSpinklerChanged()
    {
        base.HandleSpinklerChanged();
        if(vfx != null)
        {
            curSpeed = originSpeed * (float)(Spinkler / 100);
            curLifeTime = originLT * (float)(Spinkler / 100) * (float)(Spinkler / 100);
            //main.startSpeed = curSpeed;
            main.startLifetime = curLifeTime;
        }
    }
}
