using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCollaps : MonoBehaviour
{
    public CeilingFan fan;
    public float timeStartActive = 3;
    IEnumerator Start()
    {
        while (!ProgressCtrl.Instance.InProgressing)
            yield return null;

        if ((int)GameSetting.typeEquipment != 3)
            yield break;

        timeStartActive = Random.Range(10, 20); //GameConfig.TestConfig.isActivate ? 5 : Random.Range(10, 60);

        float countTime = 0;
        while (countTime < timeStartActive)
        {
            if (ProgressCtrl.Instance.InProgressing)
            {
                countTime += 1;
                yield return new WaitForSeconds(1);
            }
            else
                yield break;
        }
        print("Fan active!!!!!!!!");
        fan.Active();
    }
}
