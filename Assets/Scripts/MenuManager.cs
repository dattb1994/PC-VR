using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public bool isTestMode = false;
    public TypeEquip typeEquipment;
    public Transform gridRoom;

    //List<string> namesGame;
    public Text txtNameGame;

    [Header("SelectDevice")]
    [SerializeField] GameObject panelSelectDevice;
    [SerializeField] Button btnSelectDeviceBinh, btnSelectDeviceLang;
    [Header("SelectLoaiBinh")]
    [SerializeField] GameObject panelSelectLoaiBinh;
    [SerializeField] TypeEquip selectBinh = 0;

    [Header("SelectMoiTruong")]
    [SerializeField] GameObject panelSelectMoiTruong;
    [SerializeField] int selectMoiTruong = 0;

    [Header("LoadScene")]
    [SerializeField] GameObject panelLoading;
    [SerializeField] Slider sliderProcess;
    [SerializeField] GameObject helpPanel;
    [SerializeField] GameObject panelThuKetNoiLaiServer;

    public List<HideIfIsLang> hideIfIsLangs = new List<HideIfIsLang>();

    IEnumerator Start()
    {
        SwitchSceneBlurEffect.Instance.ForceShowBlur();
        QualitySettings.SetQualityLevel(1, true);
        Application.targetFrameRate = 72;
        yield return new WaitForEndOfFrame();

        //txtNameGame.text = namesGame[typeEnv];
        //print("Name Game: " + namesGame[typeEnv]);
        hideIfIsLangs = Resources.FindObjectsOfTypeAll<HideIfIsLang>().ToList();// FindObjectsByType<HideIfIsLang>(FindObjectsSortMode.None).ToList();
        var all = Resources.FindObjectsOfTypeAll<HideIfIsLang>();
        foreach (var t in all)
        {
            if (t.gameObject.scene.IsValid()) // Nếu object thuộc scene
            {
                hideIfIsLangs.Add(t);
            }
        }
        for (int i = 0; i < gridRoom.childCount; i++)
        {
            gridRoom.GetChild(i).gameObject.SetActive(i < roomSetting.maxRoom);
        }
        yield return new WaitForEndOfFrame();
        SwitchSceneBlurEffect.Instance.HideBlur();
    }
    public void KhoiTaoGame()
    {
        GameSetting.typeEquipment = selectBinh;
        GameSetting.typeEnv = (int)roomSetting.typeEnv;

        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        SwitchSceneBlurEffect.Instance.ShowBlur();
        yield return new WaitForSeconds(1);
        GameSetting.sceneToLoad = "Main";
        SceneManager.LoadScene(0);
    }
    public void SetDevice(int index)
    {
        selectBinh = (TypeEquip)index;

        for (int j = 0; j < hideIfIsLangs.Count; j++)
        {
            hideIfIsLangs[j].gameObject.SetActive(selectBinh != TypeEquip.Steering); 
        }
    }


    public RoomSetting roomSetting;
}
[System.Serializable]
public struct RoomSetting
{
    public TypeRoom typeEnv;
    public int maxRoom;
    public string version;
    public int bundleVersionCode;
}
public enum TypeRoom
{
    office = 0,
    apartment_living_room = 1,
    living_room_with_garage = 2,
    kitchen = 3,
    bedroom = 4,
    warehouse = 5
}