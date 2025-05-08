using Autohand;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    public UnityEvent onTouchBegin;
    Vector3 startScale;
    HandTouchEvent touchEvent;

    public bool interact = true;

    //public MenuMain menuMain;
    private void Start()
    {
        //touchEvent = GetComponent<HandTouchEvent>();
        //touchEvent.HandStartTouch.AddListener((Hand) =>
        //{
        //    if (!interact)
        //        return;

        //    transform.DOScale(startScale * .8f, .2f);
        //    DOVirtual.DelayedCall(.2f, () =>
        //    {
        //        transform.DOScale(startScale, .2f);
        //        onTouchBegin?.Invoke();
        //    });
        //});
        startScale = transform.localScale;
    }
    public void OnTouchBegin(ToucherIndex toucher)
    {
        if (!interact)
            return;

        transform.DOScale(startScale * .8f, .2f);
        DOVirtual.DelayedCall(.2f, () =>
        {
            transform.DOScale(startScale, .2f);
            onTouchBegin?.Invoke();
        });
        toucher.JustClick();

        //menuMain.JustClick();
    }
}
