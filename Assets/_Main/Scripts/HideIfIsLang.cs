using UnityEngine;

public class HideIfIsLang : MonoBehaviour
{
    private void OnEnable()
    {
        //if(MenuManager.Instance.hideIfIsLangs.Contains(this))
        //    MenuManager.Instance.hideIfIsLangs.Add(this);

        ////Toggle(GameSetting.typeEquipment != TypeEquip.Steering);
    }
    private void Update()
    {
        //gameObject.SetActive(GameSetting.typeEquipment != 3);
    }
    public void Toggle(bool isShow)
    { 
        gameObject.SetActive(isShow); 
    }
}
