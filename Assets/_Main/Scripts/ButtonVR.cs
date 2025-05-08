using DG.Tweening;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonVR : UiElementVR
{
    public override void DoOnClick()
    {
        if (!GetComponent<Button>().interactable)
            return;

        base.DoOnClick();
        GetComponent<Button>().onClick.Invoke();
    }
}
