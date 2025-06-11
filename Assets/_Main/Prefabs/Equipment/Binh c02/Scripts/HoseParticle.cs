using Ignis;
using System;
using System.Collections;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

public class HoseParticle : MonoBehaviour
{
    public ParticleSystem vfx;
    public UnityAction<float> onSpinklerChanged;
    public AudioSource _audio;

    [SerializeField] bool isOn;

    public virtual void Start()
    {
        if (GameManager.Instance)
        {
            var equipStr = GameManager.Instance.equipConfig;
            vfx.GetComponent<ParticleExtinguish>().particleExtinquishRadius = equipStr.radius;
            vfx.GetComponent<ParticleExtinguish>().incrementalPower = equipStr.power;
        }
        EventManager.StartListening(EventKey.onEffectIntensityChanged.ToString(), OnEffectIntensityChanged);
    }

    internal virtual void OnEffectIntensityChanged()
    {
        effectIndensity = GameSetting.effectIntensity;
    }

    private void Update()
    {
        if(IsOn && !vfx.isPlaying)
        {
            vfx.Play();
        }
    }
    public bool IsOn
    {
        set
        {
            if (isOn != value)
            {
                isOn = value;
                HandleIsOnChanged();
                if (isOn && vfx != null)
                {
                    //vfx.gameObject.SetActive(true);
                    //vfx.Play();
                    //StartCoroutine(ForcePlayVfx());
                }
                else
                {
                    vfx.Stop();   
                    //vfx.gameObject.SetActive(false);
                }
                OnToggleChanged?.Invoke(isOn);
            }
        }
        get => isOn;
    }
    public virtual void HandleIsOnChanged()
    {
        
    }

    [SerializeField] float spinkler;
    public float Spinkler
    {
        set
        {
            if (spinkler != value)
            {
                spinkler = value;
                HandleSpinklerChanged();
            }
        }
        get
        { 
            float v = spinkler * Mathf.Lerp(0.5f, 2, effectIndensity);
            return v;
        }
    }
    public float effectIndensity;
    public UnityAction<bool> OnToggleChanged;
    private void OnEnable()
    {
        SetOnEnable();
        effectIndensity = GameSetting.effectIntensity;
    }
    public virtual void SetOnEnable()
    {
        if (vfx != null)
        {
            vfx.Stop();
            var emission = vfx.emission;
            emission.rateOverTime = 0;
        }
    }
    public virtual void HandleSpinklerChanged()
    {
        if (vfx != null)
        {
            var emission = vfx.emission;
            emission.rateOverTime = Spinkler * deltaFixed ;
            onSpinklerChanged?.Invoke(Spinkler);
        }
        if (_audio != null)
        { 
            if(Spinkler > 0)
            {
                if (!_audio.isPlaying)
                {
                    _audio.Play();
                }
            }
            else
            {
                if (_audio.isPlaying)
                {
                    _audio.Stop();
                }
            }
        }
    }
    public float deltaFixed = 1;
}