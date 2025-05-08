using Autohand;
using System;
using UnityEngine;

public class XoayDauVoi : MonoBehaviour
{
    Grabbable grabbable;
    private HingeJoint hinge;
    private bool isGrabbing = false;
    Rigidbody rb;

    public Vector2 limit = new Vector2(0, 90);

    public bool IsOn = false;
    public float threshold = 5;

    public float angle;
    public bool autoLockWhenRelease = true;

    public float Spinkler;

    //public Transform pivot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);

        hinge = GetComponent<HingeJoint>();
        JointLimits limits = hinge.limits;
        limits.min = limit.x;
        limits.max = limit.y;
        hinge.limits = limits;

        LockAngle(limit.y);

        //transform.SetParent(pivot);
    }
    private void Update()
    {
        if (IsOn)
        {
            if (Math.Abs(limit.y - hinge.angle) <= threshold)
                IsOn = false;
        }
        else
        {
            if (Math.Abs(limit.x - hinge.angle) <= threshold)
                IsOn = true;
        }

        if(FireEquipmentMng.Instance)
            FireEquipmentMng.Instance.lucDangBop = IsOn ? 100 : 0;

        angle = hinge.angle;
        Spinkler = ConvertToPercentage(angle, limit.x, limit.y);
    }
    //private void LateUpdate()
    //{
    //    transform.localPosition = Vector3.zero;
    //    transform.localRotation = Quaternion.identity;
    //}
    float ConvertToPercentage(float x, float min, float max)
    {
        if (max == min) return 0; // Tránh chia cho 0
        return Mathf.Clamp((x - min) / (max - min) * 100f, 0f, 100f);
    }
    public void OnGrab(Hand arg0, Grabbable arg1)
    {
        isGrabbing = true;
        PoolingsMng.Instance.Play(3);
        UseLimit();
    }
    public void OnRelease(Hand arg0, Grabbable arg1)
    {
        isGrabbing = false;
        float angle = hinge.angle;

        //if (autoLockWhenRelease)
        {
            float distToMin = Mathf.Abs(angle - (limit.x)); // Khoảng cách đến -5
            float distToMax = Mathf.Abs(angle - limit.y); // Khoảng cách đến 75

            if (distToMin < distToMax)
            {
                Debug.Log("x gần -5 hơn");
                LockAngle(limit.x);
            }
            else
            {
                LockAngle(limit.y);
            }
        }
        //else
            //LockAngle(angle);
    }
    void LockAngle(float angle)
    {
        JointSpring spring = hinge.spring;
        spring.spring = 1000000f; // Lực cứng khi khóa lại
        spring.damper = 100f;   // Giảm rung lắc
        spring.targetPosition = angle;
        hinge.spring = spring;
        hinge.useSpring = true;
    }
    void UseLimit()
    {
        hinge.useLimits = true;
        hinge.useSpring = false;
    }

}
