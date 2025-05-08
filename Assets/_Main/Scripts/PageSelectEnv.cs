using UnityEngine;
using UnityEngine.UI;

public class PageSelectEnv : MonoBehaviour
{
    public GameObject panelLevel;
    public Transform rootBtnVanPhong;

    private void Start()
    {
        for (int i = 0; i < MenuManager.Instance.roomSetting.maxRoom; i++)
        {
            int index = i + 1;
            var btn = rootBtnVanPhong.GetChild(i).GetComponent<Button>();
            btn.onClick.AddListener(()=> SelectEnvIndex(index));
            btn.transform.GetChild(0).GetComponent<Text>().text = $"Lesson {index}";
        }
    }
    public void SelectEnviType(int index)
    {
        GameSetting.typeEnv = index;
    }
    public void SelectEnvIndex(int index)
    {
        GameSetting.indexEnv = index;
        ShowLevel();
    }
    public void ShowLevel()
    {
        gameObject.SetActive(false);
        panelLevel.SetActive(true);
    }
}
[System.Serializable]
public struct SEnviType
{
    public GameObject vanPhong;
    public GameObject nhaO;
    public GameObject khoHang;
}
