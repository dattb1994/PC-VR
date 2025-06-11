using Autohand;
using DG.Tweening;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class AutoGrab : MonoBehaviour
{
    [SerializeField] Hand handRight;
    GrabbablePoseAnimaion grabbablePoseAnimaion;

    Grabbable grabbable;
    public Transform handle;
    public float spinkler = 0;
    public bool chotBinh = true;

    public int countGrabing = 0;

    private Rigidbody m_rig;
    Rigidbody rig
    {
        get
        { 
            if(m_rig == null)
                m_rig = GetComponent<Rigidbody>();
            return m_rig;
        }
    }
    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        
        grabbable.onRelease.AddListener(Release);
        grabbable.onGrab.AddListener(Grab);
        grabbablePoseAnimaion = GetComponent<GrabbablePoseAnimaion>();
        if (grabbablePoseAnimaion != null)
        {
            grabbablePoseAnimaion.enabled = false;
        }
    }
    private void Grab(Hand arg0, Grabbable arg1)
    {
        rig.isKinematic = false;

        if(arg0.gameObject.CompareTag("hand right"))
            handRight = arg0;
        countGrabing++;
        if (countGrabing > 2)
            countGrabing = 2;
    }
    private void Release(Hand arg0, Grabbable arg1)
    {
        if (arg0.gameObject.CompareTag("hand right"))
        {
            handRight = null;
            spinkler = 0;
        }
        countGrabing--;
        if (countGrabing < 0)
            countGrabing = 0;
        EventManager.EmitEvent(EventKey.onEquipmentThrow.ToString());
    }
    private void Update()
    {
        if (countGrabing == 0)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        if (FireEquipmentMng.Instance)
            FireEquipmentMng.Instance.lucDangBop = spinkler;
        if (handRight == null || chotBinh)
        {
            spinkler = 0;
            return;
        }

        spinkler = handRight.GetSqueezeAxis() * 100;
    }
    public void MoChotBinh()
    {
        chotBinh = false;
        if (grabbablePoseAnimaion != null)
        {
            grabbablePoseAnimaion.enabled = true;
        }
    }
}
