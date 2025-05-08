using Ignis;
using System.Collections;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCtrl : MonoBehaviour
{
    public static ProgressCtrl Instance { get; private set; }
    public float countTime = 0;
    [SerializeField] bool inProgressing = false;
    public bool InProgressing
    {
        set
        {
            if (inProgressing != value)
            {
                inProgressing = value;
                EventManager.EmitEventData(EventKey.onInProgressingChaned.ToString(), value);
            }
        }
        get => inProgressing;
    }
    public FireEquipment currentEquip;
    public Transform canvasMain;
    public Transform canvasProgress;

    [Header("Binh cuu hoa")]
    public GameObject hintCheckShake;
    public GameObject hintCheckAngle;
    public GameObject hintDistanceFirePoint;
    public GameObject hintGiatChot;
    public GameObject hintHuongVeNgonLua;
    public GameObject forceSuccessUI;
    public float floorHigh = 0;
    public float lchWithfloorHigh = 0;

    [Header("Lang chua chay")]
    public GameObject hintB1_1;
    public GameObject hintB1_2;
    public GameObject hintB1_3;
    public GameObject hintB2;
    public ThuLang thuLang;

    public EndProgressUI enProgressUI;
    public GameObject warningSapCauKienUI;
    public bool isSuccess = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {                                    
        forceSuccessUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!EnvCtrl.Instance.IsExtinguished())
                ScoreMechanic.Instance.ForceOverTime();

            //currentEquip.Hose.IsOn = false;
            forceSuccessUI.gameObject.SetActive(false);
            forceSuccessUI.transform.GetChild(0).gameObject.SetActive(false);
            forceSuccessUI.transform.GetChild(1).gameObject.SetActive(true);
            isSuccess = true;
        });
    }
    private void Update()
    {
        if (InProgressing)
        {
            if (countTime < EnvCtrl.Instance.maxTimePlay + 1)
                countTime += Time.deltaTime;

            if (ScoreMechanic.Instance.isOvertime || EnvCtrl.Instance.IsExtinguished() || isSuccess)
            {
                print("------------------StopProgress-------------------");
                StopProgress();
            }
        }
    }
    Coroutine coStartProgress;
    public void StartProgress()
    {
        StartCoroutine(IEStartProgress());
    }
    public void StopProgress()
    {
        //FlameEngine.instance.PauseFlames();

        if (coStartProgress != null)
        {
            StopCoroutine(coStartProgress);
            coStartProgress = null;
        }

        hintHuongVeNgonLua.SetActive(false);
        canvasProgress.gameObject.SetActive(false);

        var cam = Camera.main.transform;
        Vector3 camForward = cam.position + cam.forward;
        Vector3 posC = new Vector3(camForward.x, cam.position.y, camForward.z);
        canvasMain.position = posC;
        canvasMain.LookAt(cam, Vector3.up);
        canvasMain.eulerAngles += new Vector3(0, 180, 0);
        canvasMain.gameObject.SetActive(true);

        ScoreMechanic.Instance.isActivating = false;
        InProgressing = false;
        enProgressUI.Init(countTime, (int)ScoreMechanic.Instance.TinhDiem());
        GameManager.Instance.studentBodyLog.finishOn = System.DateTime.Now.ToString();
        GameManager.Instance.studentBodyLog.point = (int)ScoreMechanic.Instance.TinhDiem();
        GameManager.Instance.studentBodyLog.note = ScoreMechanic.Instance.ToString();
        //GameManager.Instance.LogPracticeStudent();
    }

    IEnumerator IEStartProgress()
    {
        
        yield return new WaitForSeconds(1);
        InProgressing = true;
        GameManager.Instance.studentBodyLog.startOn = System.DateTime.Now.ToString();
        ScoreMechanic.Instance.isActivating = true;
        while (currentEquip == null)
            yield return null;
        if (GameSetting.level < 16)
        {
            if (currentEquip.GetComponent<Binh1>() != null)
                coStartProgress = StartCoroutine(ProgressBinh());
            else if (currentEquip.GetComponent<FireSteering>() != null)
                coStartProgress = StartCoroutine(ProgressFireSteering());
        }

    }
    IEnumerator ProgressBinh()
    {
        Binh1 binhCuuHoa = currentEquip.GetComponent<Binh1>();
        //binhCuuHoa.Hose.IsOn = false;

        if (binhCuuHoa.type == TypeBinh.flour)
        {
            ///Check sóc bình
            hintCheckShake.gameObject.SetActive(true);
            binhCuuHoa.shakingChecker.StartChecking();
            while (!binhCuuHoa.shakingChecker.shook) yield return null;
            hintCheckShake.SetActive(false);
        }

        ///Cách đám cháy 4m
        hintDistanceFirePoint.SetActive(true);
        yield return new WaitForSeconds(3);
        hintDistanceFirePoint.SetActive(false);

        /////Nghiêng góc 60 độ so với mặt đất để giật chốt
        //hintCheckAngle.SetActive(true);
        ////while (binhCuuHoa.Angle() < 45 || binhCuuHoa.Angle() > 60) yield return null;
        //yield return new WaitForSeconds(3);
        //hintCheckAngle.SetActive(false);

        ///Giật chốt 
        currentEquip.GetComponent<Binh1>().chotBinh.SetBeckForce(1000);
        hintGiatChot.SetActive(true);
        while(!currentEquip.Hose.IsOn)
            yield return null;

        currentEquip.Hose.IsOn = true;
        hintGiatChot.SetActive(false);

        ///Hướng vào gốc lửa
        hintHuongVeNgonLua.SetActive(true);
        while (currentEquip.Hose.Spinkler <= 0)
            yield return null;
        forceSuccessUI.SetActive(true);
        ///Bột phủ toàn bộ đám cháy

        ///Hô “Xong”
        //while (!isSuccess) yield return null;
        hintHuongVeNgonLua.SetActive(false);
    }
    IEnumerator ProgressFireSteering()
    {
        FireSteering fireSteering = currentEquip.GetComponent<FireSteering>();
        //fireSteering.Hose.IsOn = false;
        yield return new WaitForSeconds(1);

        hintB1_1.SetActive(true);
        while (!fireSteering.Hose.IsOn)
            yield return null;
        hintB1_1.SetActive(false);
        yield return new WaitForSeconds(1);

        bool isTried = false;
        thuLang.OnStried += () => isTried = true;
        thuLang.isTrying = true;
        hintB1_2.SetActive(true);
        while (!isTried)
        {
            yield return null;
        }
        hintB1_2.SetActive(false);
        yield return new WaitForSeconds(1);

        hintB1_3.SetActive(true);
        while (fireSteering.Hose.IsOn)
            yield return null;
        hintB1_3.SetActive(false);
        yield return new WaitForSeconds(1);

        hintB2.SetActive(true);
        while (!fireSteering.Hose.IsOn)
            yield return null;
        hintB2.SetActive(false);

        yield return new WaitForSeconds(3);
        forceSuccessUI.SetActive(true);
        ///Bột phủ toàn bộ đám cháy

        ///Hô “Xong”
        //while (!isSuccess) yield return null;
        //Finished((int)ScoreMechanic.Instance.TinhDiem());
    }
}
