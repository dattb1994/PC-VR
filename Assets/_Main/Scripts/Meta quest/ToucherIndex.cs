using DG.Tweening;
using Oculus.Interaction;
using System;
using UnityEngine;

public class ToucherIndex : MonoBehaviour
{
    Vector3 originScale;
    public float radius = 0.1f;
    private void Start()
    {
        originScale = transform.localScale;
    }
    private void Update()
    {
        var col = Physics.OverlapSphere(transform.position, radius);
        if (col != null)
        {
            foreach (var item in col)
            {
                var btn = item.gameObject.GetComponent<Button3D>();
                if (btn != null)
                {
                    btn.OnTouchBegin(this);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    internal void JustClick()
    {
        transform.DOScale(originScale * .8f, .2f);
        DOVirtual.DelayedCall(.2f, () =>
        {
            transform.DOScale(originScale, .2f);
        });
    }
}
