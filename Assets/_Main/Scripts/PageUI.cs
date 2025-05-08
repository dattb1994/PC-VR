using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : MonoBehaviour
{
    private void OnEnable()
    {
        OnShow();
    }
    public void OnShow()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == 0);
        }
    }
}
