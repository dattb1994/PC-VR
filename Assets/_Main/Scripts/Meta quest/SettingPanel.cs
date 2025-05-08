using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Slider effectIntensitySlider;
    private void OnEnable()
    {
        effectIntensitySlider.value = GameSetting.effectIntensity;
    }
    private void Start()
    {
        effectIntensitySlider.onValueChanged.AddListener(OnEffectIntensityChanged);
    }

    private void OnEffectIntensityChanged(float arg0)
    {
        GameSetting.effectIntensity = effectIntensitySlider.value;
    }
}
