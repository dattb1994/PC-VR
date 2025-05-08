using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLevelUI : MonoBehaviour
{
    public Button btnCong, btnTru, btnKhoitaoGame;
    public Text txtLV;

    public Transform rootbtns;
    public List<Button> btns;
    private void OnEnable()
    {
        UpdateUI();
    }
    private void Start()
    {
        GameSetting.level = 1;

        btnCong.onClick.AddListener(()=> ChangeLevel("+"));
        btnTru.onClick.AddListener(() => ChangeLevel("-"));
        btnKhoitaoGame.onClick.AddListener(KhoiTaoGame);

        for (int i = 0; i < rootbtns.childCount; i++)
        {
            btns.Add(rootbtns.GetChild(i).GetComponent<Button>());
        }

        for (int i = 0; i < btns.Count; i++)
        {
            int index = i+1;
            btns[i].onClick.AddListener(()=> OnClick(index));
            btns[i].transform.GetChild(0).GetComponent<Text>().text = (index).ToString();
        }
        UpdateUI();
    }
    public void OnClick(int i)
    {
        GameSetting.level = i;
        
        UpdateUI();
    }

    public void ChangeLevel(string type)
    {
        GameSetting.level += type == "+" ? 1 : -1;
        int lv = Mathf.Clamp(GameSetting.level, 1, 10);

        GameSetting.level = lv;                                                                        
        UpdateUI();
    }
    public Color[] colors; 
    void UpdateUI()
    {
        print($"Level: {GameSetting.level}");
        //txtLV.text = GameSetting.level.ToString();
        //btnCong.gameObject.SetActive(GameSetting.level < 10);
        //btnTru.gameObject.SetActive(GameSetting.level > 1);
        for (int j = 0; j < btns.Count; j++)
        {
            btns[j].GetComponent<Image>().color = GameSetting.level - 1 == j ? colors[0]: colors[1];
        }
    }
    public void KhoiTaoGame()
    {
        MenuManager.Instance.KhoiTaoGame();
    }
}
