using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FaucetCo2 : MonoBehaviour
{
    Transform model, head, anchor;
    public Transform tracker;

    public Vector2 clampPosY;
    private void Start()
    {
        model = transform.Find("model");
        head = model.Find("head");
        anchor = transform.Find("anchor");
    }
    private void FixedUpdate()
    {
        head.transform.localPosition = new Vector3(0.115599997f, 0, -0.00200000009f);
        model.LookAt(tracker, model.up);
        head.position = anchor.position;
        head.LookAt(model, head.up);
    }
}
                                                                                                                       