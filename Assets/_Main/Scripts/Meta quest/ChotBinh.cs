using Autohand;
using System;
using TigerForge;
using UnityEngine;

public class ChotBinh : MonoBehaviour
{
    public FixedJoint fixedJoint;
    public bool hasJoint = true;
    public AutoGrab autoGrab;

    Grabbable grabbable;
    Transform originParent;
    private void Start()
    {
        SetBeckForce(10000000);
        originParent = transform.parent;
        grabbable = GetComponent<Grabbable>();
        grabbable.onRelease.AddListener(Release);
    }

    private void Release(Hand arg0, Grabbable arg1)
    {
        transform.SetParent(hasJoint ? originParent : null);
    }

    private void Update()
    {
        if (hasJoint)
        {
            if (fixedJoint == null)
            {
                hasJoint = false;
                transform.SetParent(null);
                autoGrab.MoChotBinh();
                PoolingsMng.Instance.Play(1);
            }
        }
    }
    public void SetBeckForce(int force)
    {
        fixedJoint.breakForce = force;
    }
}
