using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageNhaOUI : MonoBehaviour
{
    public GameObject main;
    public Transform rootBtn;
    public List<GameObject> types;
    public List<Transform> listIndex;
    public int delta = 1;

    public PageSelectEnv selectEnv;

    public List<Button> btnbacks;
    private void OnEnable()
    {
        main.SetActive(true);
        for (int j = 0; j < types.Count; j++)
            types[j].SetActive(false);
    }
    private void Start()
    {
        for (int i = 0; i < rootBtn.childCount; i++)
        {
            int type = i;
            rootBtn.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SellectType(type));
        }

        for (int i = 0; i < listIndex.Count; i++)
        {
            var grid = listIndex[i];
            for (int j = 0; j < grid.childCount; j++)
            {
                int index = j;
                var btn = grid.GetChild(j).GetComponent<Button>();
                btn.onClick.AddListener(() => SellectIndex(index));
                btn.transform.GetChild(0).GetComponent<Text>().text = $"Bài {index + 1}";
            }
        }

        for (int i = 0; i < btnbacks.Count; i++)
        {
            btnbacks[i].onClick.AddListener(()=>
            {
                main.SetActive(true);
                for (int j = 0; j < types.Count; j++)
                    types[j].SetActive(false);
            });
        }
    }
    public void SellectType(int index)
    {

        int type = index + delta;

        main.SetActive(false);
        GameSetting.typeEnv = type;

        for (int i = 0; i < types.Count; i++)
        {
            types[i].SetActive(i == index);
            print($"i {i} == index {index}");
        }
    }
    public void SellectIndex(int index)
    {
        selectEnv.SelectEnvIndex(index);
    }
}
