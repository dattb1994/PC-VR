
using Autohand;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{
    Grabbable grabbable;
    HingeJoint hinge;

    public Vector2 limit = new Vector2(0, 90);

    public List<SpringStruct> springStructs = new List<SpringStruct>();

    public float angle;

    private void Start()
    {
        grabbable = GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);
        hinge = GetComponent<HingeJoint>();
    }

    private void OnRelease(Hand arg0, Grabbable arg1)
    {
        angle = hinge.angle;

        //JointLimits limits = hinge.limits;
        //limits.min = angle;
        //limits.max = angle;
        //hinge.limits = limits;

        JointSpring spring = hinge.spring;
        spring.spring = springStructs[0].spring;
        spring.damper = springStructs[0].damper;
        spring.targetPosition = angle;
        hinge.spring = spring;
        hinge.useSpring = true;

    }

    private void OnGrab(Hand arg0, Grabbable arg1)
    {
        //hinge.useLimits = true;

        //JointLimits limits = hinge.limits;
        //limits.min = 0f;
        //limits.max = 90f;
        //hinge.limits = limits;

        JointSpring spring = hinge.spring;
        hinge.useSpring = false;
        hinge.spring = spring;
        return;

        //if (hinge == null)
        //{
        //    hinge = GetComponent<HingeJoint>();
        //}
        //JointSpring spring = hinge.spring;
        //spring.spring = springStructs[1].spring;
        //spring.damper = springStructs[1].damper;
        //hinge.spring = spring;
        //hinge.useSpring = true;
    }

    [System.Serializable]
    public struct SpringStruct
    {
        public float spring;
        public float damper;
    }
}
