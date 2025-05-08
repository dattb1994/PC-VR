using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckRungDauLang : MonoBehaviour
{
    public bool binhthuong = false;
    public float threshold, dis;
    Vector3 t2;
    public UnityAction onRung;
    private void Update()
    {
        dis = Vector3.Distance(transform.position, t2);
        if (binhthuong)
        {
            if (dis > threshold)
            {
                onRung?.Invoke();
                binhthuong = false;
            }
        }
        else
        {
            if (dis < threshold)
            {
                binhthuong = true;
            }
        }
    }
    private void LateUpdate()
    {
        t2 = transform.position;
    }
}
