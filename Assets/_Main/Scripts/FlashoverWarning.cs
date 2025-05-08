using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TigerForge;
using UnityEngine;

public class FlashoverWarning : Singleton<FlashoverWarning>
{
    public float timeStart = 5;
    public Vector2 timeStartThreshold = new Vector2(10, 60);
    public float timeLife = 5;
    public bool isFlashover = false;

    public List<GameObject> vfxs;

    public float delayTime, countVfx;

    public bool IsFlashover
    {
        set
        {
            isFlashover = value;
            ToggleUI(value);
        }
        get => isFlashover;
    }
    public GameObject ui;
    AudioSource audioSource;
    private void Start()
    {
        ui.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < transform.childCount; i++)
        { 
            vfxs.Add(transform.GetChild(i).gameObject);
        }
    }
    public IEnumerator StartProgress()
    {
        while (!ProgressCtrl.Instance.InProgressing) yield return null;

        timeStart = Random.Range(timeStartThreshold.x, timeStartThreshold.y);//GameConfig.TestConfig.isActivate ? 5 : Random.Range(10, 60);

        float countTime = 0;
        while (countTime < timeStart)
        {
            if (!ProgressCtrl.Instance.InProgressing)
                yield break;

            countTime += Time.deltaTime;
            yield return null;
        }
        ToggleUI(true);
        audioSource.Play();

        float t1 = 0;
        while (t1 < 2)
        {
            if (ProgressCtrl.Instance.InProgressing)
            {
                t1 += Time.deltaTime;
                yield return null;
            }
            else
                yield break;
        }
        EventManager.EmitEvent(EventKey.OnFlashover.ToString());

        for (int i = 0; i < countVfx; i++)
        {
            StartCoroutine(PlayVfx(vfxs[i]));
            float t = 0;
            while (t < delayTime)
            {
                if (ProgressCtrl.Instance.InProgressing)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    StopVFX();
                    yield break;
                } 
            }
        }
        yield return new WaitForSeconds(5);
        StopVFX();
    }
    void StopVFX()
    {
        ToggleUI(false);
        audioSource.Stop();
        IsFlashover = false;
    }

    IEnumerator PlayVfx(GameObject vfx)
    {
        vfx.transform.position = EnvCtrl.Instance.startFire.position;
        vfx.transform.LookAt(EnvCtrl.Instance.door);
        vfx.SetActive(true);
        float time = 0;
        while (time < timeLife)
        {
            if (ProgressCtrl.Instance.InProgressing)
            {
                time += Time.deltaTime;
                yield return null;
            }
            else
            {
                StopVFX();
                yield break;
            }
        }
        vfx.SetActive(false);
    }

    void ToggleUI(bool isOn)
    {
        if (GameSetting.level > 5)
            return;

        ui.SetActive(isOn);
    }
}
