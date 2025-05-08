using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(HoseParticle))]
public class FireEquipment : MonoBehaviour
{
    private HoseParticle hose;
    public virtual void Start()
    {
        if (ProgressCtrl.Instance)
        {
            ProgressCtrl.Instance.currentEquip = this;
            print(111111111);
        }
    }
    public HoseParticle Hose
    {
        get
        {
            if (hose == null)
                hose = GetComponent<HoseParticle>();
            return hose;
        }
    }

    public void ToggleActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
