using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class MenuMain : MonoBehaviour
{
    public Transform pivot;
    public Transform offset;
    public Text txtOffset;

    public List<Button3D> btns;

    private void Update()
    {
        foreach (Transform t in transform)
        { 
            if(t.gameObject.layer != LayerMask.NameToLayer("Default"))
                t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    void Start()
    { 
        foreach (Transform t in transform)
        {
            var btn = t.gameObject.GetComponent<Button3D>();
            if (btn!= null)
            {
                btns.Add(btn);
                btn.onTouchBegin.AddListener(JustClick);
            }
        }
    }

    public void JustClick()
    {
        foreach (var t in btns)
        {
            t.interact = false;
        }
        DOVirtual.DelayedCall(2, () =>
        {
            foreach (var t in btns)
            {
                t.interact = true;
            }
        });
    }

    public void Replay()
    {
        GameSetting.sceneToLoad = "Main";
        SwitchSceneBlurEffect.Instance.ShowBlur();
        DOVirtual.DelayedCall(1, () =>
        {
            SceneManager.LoadScene(0);
        });
    }
    public void ResetView()
    {

        offset.localPosition = Vector3.zero;
        offset.localRotation = Quaternion.identity;
        //offset.LookAt(EnvCtrl.Instance.startFire);

        EventManager.EmitEvent(EventKey.onResetView.ToString());

        txtOffset.transform.localScale = Vector3.one;
        DOVirtual.DelayedCall(1, () =>
        {
            txtOffset.transform.localScale = Vector3.zero;
        });
    }
    public void Menu()
    {
        GameSetting.sceneToLoad = "Menu Van phong";
        SwitchSceneBlurEffect.Instance.ShowBlur();
        DOVirtual.DelayedCall(1, () =>
        {
            SceneManager.LoadScene(0);
        });
    }
    public void Setting()
    { 
        
    }
}
