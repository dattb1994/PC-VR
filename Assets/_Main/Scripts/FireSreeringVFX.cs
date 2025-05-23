using Autohand.Demo;
using Ignis;
using TigerForge;
using UnityEngine;
public class FireSreeringVFX : HoseParticle
{
    [SerializeField] float thresholdChangeMode = .8f;
    public GameObject water, smoke, rootVfx;

    public bool isFullSuong;
    public bool IsFullSuong => isFullSuong;

    ParticleSystem vfxWater, vfxSmoke;
    public override void SetOnEnable()
    {

    }
    public override void Start()
    {
        if (GameManager.Instance)
        {
            var equipStr = GameManager.Instance.equipConfig;
            water.GetComponent<ParticleExtinguish>().particleExtinquishRadius = equipStr.radius;
            water.GetComponent<ParticleExtinguish>().incrementalPower = equipStr.power;
            print(11111);
        }
        EventManager.StartListening(EventKey.onEffectIntensityChanged.ToString(), OnEffectIntensityChanged);

        vfxWater = water.GetComponent<ParticleSystem>();
        vfxSmoke = smoke.GetComponent<ParticleSystem>();

        float v = Mathf.Lerp(0.5f, 2, effectIndensity);

        var emissionWater = vfxWater.emission;
        emissionWater.rateOverTime = 100 * v;

        var emissionSmoke = vfxSmoke.emission;
        emissionSmoke.rateOverTime = 15 * v;
    }
    internal override void OnEffectIntensityChanged()
    {
        base.OnEffectIntensityChanged();

        float v = Mathf.Lerp(0.5f, 2, effectIndensity);

        var emissionWater = vfxWater.emission;
        emissionWater.rateOverTime = 100 * v;

        var emissionSmoke = vfxSmoke.emission;
        emissionSmoke.rateOverTime = 15 * v;
    }
    public override void HandleIsOnChanged()
    {
        rootVfx.SetActive(IsOn);
    }
    public override void HandleSpinklerChanged()
    {
        if (isFullSuong)
        {
            if (Spinkler < thresholdChangeMode)
            {
                isFullSuong = false;
            }
        }
        else
        {
            if (Spinkler >= thresholdChangeMode)
            {
                isFullSuong = true;
            }
        }

        water.SetActive(!isFullSuong);
        smoke.SetActive(isFullSuong);
    }
}
