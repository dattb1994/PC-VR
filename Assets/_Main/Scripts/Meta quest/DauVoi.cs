using Autohand;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class DauVoi : MonoBehaviour
{
    Rigidbody rig;
    Grabbable grab;
    public Transform head;
    public Transform gacDau;

    public bool isGrabbing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        grab = GetComponent<Grabbable>();
        grab.onRelease.AddListener(OnRelease);
        grab.onGrab.AddListener(OnGrab);
        OnRelease(null, null);
    }

    private void OnGrab(Hand arg0, Grabbable arg1)
    {
        rig.isKinematic = false;
        isGrabbing = true;
    }

    private void OnRelease(Hand arg0, Grabbable arg1)
    {
        transform.SetParent(gacDau);
        rig.isKinematic = true;
        isGrabbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrabbing)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
