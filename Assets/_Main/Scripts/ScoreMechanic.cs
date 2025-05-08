using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMechanic : Singleton<ScoreMechanic>
{
    public float score = 10;

    public Dictionary<string, int> listError = new Dictionary<string, int>();
    public List<string> errorDebugs = new List<string>();
    public void Init()
    {
        listError.Add("Must be at 100% when fighting fire.", 0);
        listError.Add("Spray distance from 4m to 1m. Spray early before 4m.", 0);
        listError.Add("Spray distance from 4m to 1m. Spray early before 7m.", 0);
        listError.Add("Spray distance from 2m to 1m. Spray early before 2m.", 0);
        listError.Add("Get closer than 1 meter.", 0);
        listError.Add("Come closer than 5m.", 0);
        listError.Add("Back to the door. Wrong direction from 15-30 degrees", 0);
        listError.Add("Back to the door. Wrong direction from more than 30 degrees", 0);
        listError.Add("Shaking head.", 0);
        listError.Add("Building collapse.", 0);
        listError.Add("Time out, not completed!", 0);
        listError.Add("Danger warning.", 0);

        var obj = new GameObject("dir");
        dir = obj.transform;
        StartCoroutine(WaitInitDoor());

        if ((int)GameSetting.typeEquipment == 3)
        {
            //checkRungDauLang.onRung += RungDauLang;
            if (GameSetting.indexEnv == 0 && GameSetting.typeEnv == 3)
                StartCoroutine(FlashoverWarning.Instance.StartProgress());
        }
    }

    public bool isActivating = false;
    public override string ToString()
    {
        string result = "";
        for (var i = 0; i < errorDebugs.Count; i++)
        {
            result += errorDebugs[i] + (i == errorDebugs.Count - 1 ? "" : Environment.NewLine);
        }
        return result;
    }
    IEnumerator WaitInitDoor()
    {
        while (door == null)
        {
            if (EnvCtrl.Instance.door != null)
                door = EnvCtrl.Instance.door;
            yield return null;
        }

    }
    private void Update()
    {
        if (!isActivating) return;

        if ((int)GameSetting.typeEquipment != 3)
        {
            CheckSpinkler();
        }
        else
        {
            if (GameSetting.indexEnv == 0 && GameSetting.typeEnv == 3)
                CheckFlashover();
        }

        //CheckLungQuayVeCua();
        CheckKhoangCachPhun();
        CheckTienSatQua1m();
        CheckOverTime();

        disPlayToFire = distanceToFire;
    }

    /// <summary>
    /// Cảnh báo rò rỉ khí gas.
    /// </summary>
    public void CheckFlashover()
    {
        if(fireSreeringVFX.isFullSuong)
        {
            if(!dangXitDangPhunSuong)
            {
                dangXitDangPhunSuong = true;
            }
        }                                    
        else
        {
            if(dangXitDangPhunSuong)
            {
                if (flashoverWarning.IsFlashover)
                {
                    AddError("Danger warning.");
                    phatLoiFlashoverFirst = true;
                }
                dangXitDangPhunSuong = false;
            }
            if(flashoverWarning.IsFlashover && !phatLoiFlashoverFirst)
            {
                AddError("Danger warning.");
                phatLoiFlashoverFirst = true;
            }
        }
    }
    public FlashoverWarning flashoverWarning;
    public bool dangXitDangPhunSuong = false;
    public bool phatLoiFlashoverFirst = false;
    public FireSreeringVFX fireSreeringVFX;

    /// <summary>
    /// -Rung lắc đầu lăng: - 0.1
    /// </summary>
    void RungDauLang()
    {
        if (dangbop)
        {
            AddError("Shaking head.", .1f);
        }
    }
    //public CheckRungDauLang checkRungDauLang;

    /// <summary>
    /// Must be at 100% when fighting fire. Mỗi lần nhấp nhả điểm-0.25
    /// </summary>
    void CheckSpinkler()
    {
        if (dangbop100)
        {
            if (FireEquipmentMng.Instance.lucDangBop < 100)
            {
                dangbop100 = false;
                AddError("Must be at 100% when fighting fire.");
            }
        }
        else
        {
            if (FireEquipmentMng.Instance.lucDangBop == 100)
                dangbop100 = true;
        }
    }
    public bool dangbop100 = false;

    /// <summary>
    /// KHoảng cách phun// bắt đầu bóp => kiểm tra khoảng cách
    /// </summary>
    /// <param name="key"></param>
    void CheckKhoangCachPhun()
    {
        if (dangbop)
        {
            if (FireEquipmentMng.Instance.lucDangBop > 0)
            {
                dangbop = true;

                float threshold = GameSetting.typeEquipment == 0 ? 4 : (int)GameSetting.typeEquipment == 3 ? 7 : 2;
                if (distanceToFire > threshold)
                {
                    AddError(GameSetting.typeEquipment == 0 ? "Spray distance from 4m to 1m. Spray early before 4m." :
                        (int)GameSetting.typeEquipment == 3 ? "Spray distance from 4m to 1m. Spray early before 7m." :
                        "Spray distance from 2m to 1m. Spray early before 2m.", .5f);
                }
            }
        }
        else
        {
            if (FireEquipmentMng.Instance.lucDangBop > 0)
            {
                dangbop = false;
            }
        }
    }
    public bool dangbop = false;
    float distanceToFire => Vector3.Distance(player.position, EnvCtrl.Instance.startFire.position);
    public float disPlayToFire;
    /// <summary>
    /// Tiến sát quá 1m
    /// </summary>
    /// <param name="key"></param>
    void CheckTienSatQua1m()
    {
        float threshold = (int)GameSetting.typeEquipment == 3 ? 5 : 1;
        if (satqua1m)
        {
            if (distanceToFire > threshold)
            {
                satqua1m = false;
            }
        }
        else
        {
            if (distanceToFire <= threshold)
            {
                satqua1m = true;
                AddError((int)GameSetting.typeEquipment == 3 ? "Come closer than 5m." : "Get closer than 1 meter.", .5f);
            }
        }
    }
    public bool satqua1m = false;
    Transform dir;
    public Transform player;
    public Transform door;
    public float angleLungVaCua;
    public Transform cam;
    void CheckLungQuayVeCua()
    {
        dir.position = cam.position;
        dir.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
        angleLungVaCua = Vector3.Angle(dir.position - new Vector3(door.position.x, dir.position.y, door.position.z), dir.forward);

        if (lungQuayVeCua != LungQuayVeCua.duoi15)
        {
            if (angleLungVaCua < 15)
            {
                lungQuayVeCua = LungQuayVeCua.duoi15;
            }
        }
        else if (lungQuayVeCua != LungQuayVeCua.tu15den30)
        {
            if (angleLungVaCua >= 30 && angleLungVaCua <= 60)
            {
                lungQuayVeCua = LungQuayVeCua.tu15den30;
                AddError("Back to the door. Wrong direction from 15-30 degrees");
            }
        }
        else if (lungQuayVeCua != LungQuayVeCua.tren30)
        {
            if (angleLungVaCua > 30)
            {
                lungQuayVeCua = LungQuayVeCua.tren30;
                AddError("Back to the door. Wrong direction from more than 30 degrees", .5f);
            }
        }

    }
    public LungQuayVeCua lungQuayVeCua = LungQuayVeCua.duoi15;
    public bool isOvertime = false;
    public void CheckOverTime()
    {
        if (isOvertime)
            return;

        if (ProgressCtrl.Instance.countTime > EnvCtrl.Instance.maxTimePlay)
        {
            ForceOverTime();
        }
    }    
    public void ForceOverTime()
    {
        if (isOvertime)
            return;

        isOvertime = true;
        AddError("Time out, not completed!");
    }

    /// <summary>
    /// Tính điểm
    /// </summary>
    /// <param name="key"></param>
    /// <param name="scoreIncrease"></param>
    public float TinhDiem()
    {
        if (ProgressCtrl.Instance.countTime > EnvCtrl.Instance.maxTimePlay)
            return 0;

        return score * (EnvCtrl.Instance.maxTimePlay + bestTimeFire() - ProgressCtrl.Instance.countTime) / EnvCtrl.Instance.maxTimePlay;
    }

    public float bestTimeFire()
    {
        return 1;
    }

    public void AddError(string key, float scoreIncrease = .25f)
    {
        listError[key]++;
        print("Error: " + key);
        errorDebugs.Clear();

        foreach (var item in listError)
        {
            if (item.Value > 0)
                errorDebugs.Add(item.Key + $": x{item.Value}");
        }

        score -= scoreIncrease;
    }
}
public enum LungQuayVeCua
{
    duoi15, tu15den30, tren30
}
