using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCard : MonoBehaviour
{
    public Text t1, t2;
    public void Init(string key, string value)
    {
        t1.text = key;
        t2.text = value;
    }
}
