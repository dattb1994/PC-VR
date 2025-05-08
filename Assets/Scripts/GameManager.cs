using Autohand;
using DG.Tweening;
using Ignis;
using Meta.Voice.Net.WebSockets;
using System;
using System.Collections;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnvCtrl;

public class GameManager : Singleton<GameManager>
{
    public LogPracticeStudentBody studentBodyLog;
    public GameObject fireVFX;

    public int level, typeEnv, indexEnv;
    public TypeEquip indexEquip;
    public Transform posEquip;
    public Transform player;

    public string practiceCode;
    public string envCode;
    public EquipFixStr equipConfig;

    public MenuMain menuMain;

    public Transform xrOrigin;
    public GameObject hintMenu, hintResetView;

    bool isResetView = false;

    IEnumerator Start()
    {
        QualitySettings.SetQualityLevel(GameConfig.FlameConfig.smokeConfig.gameplayQuality, true);
        Application.targetFrameRate = 72;
        yield return new WaitForEndOfFrame();

        EventManager.StartListening(EventKey.onResetView.ToString(), ResetViewHandle);
        EventManager.StartListening(EventKey.onEquipmentThrow.ToString(), EquipmentThrowHandle);
        SwitchSceneBlurEffect.Instance.ForceShowBlur();
        if (GameConfig.TestConfig.isTestMode)
        {
            GameSetting.typeEnv = GameConfig.TestConfig.typeEnv;
            GameSetting.typeEquipment = GameConfig.TestConfig.typeEquipment;
            GameSetting.indexEnv = GameConfig.TestConfig.indexEnv;
            GameSetting.level = GameConfig.TestConfig.level;
        }

        foreach (var hand in FindObjectsByType<Hand>(FindObjectsSortMode.None))
        {
            hand.OnGrabbed += OnGrabbed;
            hand.OnReleased += OnGrabbed;
        }

        level = GameSetting.level;
        typeEnv = 0;
        indexEnv = GameSetting.indexEnv;
        indexEquip = GameSetting.typeEquipment;

        yield return SetupEnv();

        while (EnvCtrl.Instance == null || EnvCtrl.Instance.playerRoot == null)
            yield return null;

        SetupTransformPlayer();
        SetupEquip();
        ScoreMechanic.Instance.Init();
        ProgressCtrl.Instance.StartProgress();
        if (ApiFromCms.Instance != null)
        {
            practiceCode = ApiFromCms.Instance.practiceGetAll.objectData[(int)GameSetting.typeEquipment].code;
            envCode = ApiFromCms.Instance.environmentGetAll.objectData[GameSetting.typeEnv].code;
            studentBodyLog = new LogPracticeStudentBody(
                    userId: ApiFromCms.Instance.userLogin.objectData.currentUserId.ToString(),
                    practiceCode: ApiFromCms.Instance.practiceGetAll.objectData[(int)GameSetting.typeEquipment].code,
                    environmentCode: ApiFromCms.Instance.environmentGetAll.objectData[GameSetting.typeEnv].code,
                    logContent: "",
                    startOn: convertTime(System.DateTime.Now),
                    finishOn: convertTime(System.DateTime.Now),
                    point: 100,
                    mapCode: "" + GameSetting.indexEnv,
                    difficultLevel: GameSetting.level,
                    note: "",
                    clientId: ApiFromCms.Instance.userLogin.objectData.clientId,
                    currentUserId: ApiFromCms.Instance.adminLogin.objectData.currentUserId,
                    isAdmin: ApiFromCms.Instance.adminLogin.objectData.isAdmin,
                    tokenKey: ApiFromCms.Instance.adminLogin.objectData.tokenKey,
                    currentLanguage: ApiFromCms.Instance.userLogin.objectData.currentLanguage,
                    formObject: null); 
        }
        yield return new WaitForSeconds(2);
        menuMain.ResetView();
        menuMain.ResetView();
        menuMain.ResetView();

        EventManager.StartListening(EventKey.onResetView.ToString(), HideHintResetView);
        SwitchSceneBlurEffect.Instance.HideBlur();
        hintMenu.gameObject.SetActive(true);
        hintResetView.gameObject.SetActive(false);
    }


    private void EquipmentThrowHandle()
    {
        DOVirtual.DelayedCall(.2f, ResetViewHandle);
    }

    private void ResetViewHandle()
    {
        print("ResetView equipObj");
        Vector3 cam = Camera.main.transform.position;
        Vector3 B = EnvCtrl.Instance.startFire.position;
        Vector3 A = new Vector3(cam.x, B.y, cam.z);
        float d = .4f;
        Vector3 x = A + (B - A).normalized * d;

        Rigidbody rig;
        if (indexEquip == TypeEquip.Steering)
        {
            rig = equipObj.transform.GetChild(0).GetComponent<Rigidbody>();
            x += new Vector3(0, .2f, 0);
        }
        else
        {
            rig = equipObj.GetComponent<Rigidbody>();
        }
        rig.isKinematic = true;
        rig.transform.DOMove(new Vector3(x.x, player.position.y, x.z), .5f);
        rig.transform.rotation = Quaternion.Euler(0, 0, 0);

    }
    public void ShowHintResetView()
    {
        if (!isResetView)
        { 
            hintResetView.SetActive(true);
        }
    }
    private void HideHintResetView()
    {
        if (!isResetView)
        {
            hintResetView.SetActive(false);
            isResetView = true;
        }
    }
    public void HideMenuHandle()
    {
        if (!isResetView)
        {
            hintResetView.SetActive(false);
        }
    }

    private void OnGrabbed(Hand hand, Grabbable grabbable)
    {
        PoolingsMng.Instance.Play(0);
    }

    string convertTime(System.DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }
    IEnumerator SetupEnv()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync($"{GameSetting.indexEnv}", LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOp.isDone);

        while (FlameEngine.instance.transform.GetChild(0).childCount == 0)
            yield return null;

        Transform parent = FlameEngine.instance.transform.GetChild(0).GetChild(0);
        GameObject go = Instantiate(fireVFX, parent);
    }
    GameObject equipObj;
    void SetupEquip()
    {
        TypeEquip typeEquip = GameSetting.typeEquipment;
        equipObj = null;
        switch (typeEquip)
        {
            case TypeEquip.Flour:
                equipObj = FireEquipmentMng.Instance.binhBot;
                equipConfig = EnvCtrl.Instance.equipStr.flour;
                break;
            case TypeEquip.Co2:
                equipObj = FireEquipmentMng.Instance.binhCo2;
                equipConfig = EnvCtrl.Instance.equipStr.co2;
                break;
            case TypeEquip.Water:
                equipObj = FireEquipmentMng.Instance.binhNuoc;
                equipConfig = EnvCtrl.Instance.equipStr.water;
                break;
            case TypeEquip.Steering:
                equipObj = FireEquipmentMng.Instance.lang;
                equipConfig = EnvCtrl.Instance.equipStr.steering;
                break;
        }
        equipObj.transform.position = posEquip.position;
        equipObj.SetActive(true);
    }
    void SetupTransformPlayer()
    {
        var posPlayer = EnvCtrl.Instance.playerRoot;
        player.transform.SetPositionAndRotation(posPlayer.position, posPlayer.rotation);
    }
    public string studentBodyLogStr;
    public void LogPracticeStudent()
    {
        if (ApiFromCms.Instance != null)
        {
            studentBodyLogStr = JsonUtility.ToJson(studentBodyLog);
            StartCoroutine(ApiFromCms.Instance.IELogPracticeStudent(studentBodyLog));
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ThucHienLai()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
public enum TypeEquip
{
    Flour = 0,
    Co2 = 1,
    Water = 2,
    Steering = 3
}