using Autohand;
using System;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class FireSteering : FireEquipment
{
    public Transform vanDauVoi;
    public float thresholdOn = 95;
    public TayNam tayNam;
    public XoayDauVoi dauVoi;

    Rigidbody rig;
    public int countGrabing = 0;
    public override void Start()
    {
        base.Start();

        var grabbable = GetComponent<Grabbable>();
        rig = GetComponent<Rigidbody>();
        rig.isKinematic = true;
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);

    }

    private void OnRelease(Hand arg0, Grabbable arg1)
    {
        EventManager.EmitEvent(EventKey.onEquipmentThrow.ToString());
        HandleChangeCountGrabbing(false);
    }

    private void OnGrab(Hand arg0, Grabbable arg1)
    {
        HandleChangeCountGrabbing(true);
    }
    public void HandleChangeCountGrabbing(bool isTang)
    {
        countGrabing += isTang ? 1 : -1;
        if (countGrabing < 0)
            countGrabing = 0;
        if (countGrabing > 2)
            countGrabing = 2;

        if(countGrabing > 0)
            rig.isKinematic = false;
    }    

    private void Update()
    {
        Hose.IsOn = tayNam.IsOn;
        Hose.Spinkler = dauVoi.Spinkler;
        //Hose.Spinkler = EquipCms.Instance.@struct.m;
        //vanDauVoi.localEulerAngles = new Vector3(0, 0, -EquipCms.Instance.@struct.m);
    }
    public float DistanceWithFloor(float floor)
    { 
        return transform.position.y - floor;
    }
    public float CalculateAngle(float A)
    {
        float max = -7;
        float min = 85;

        return min + A/98 * (max - min);
    }
}

