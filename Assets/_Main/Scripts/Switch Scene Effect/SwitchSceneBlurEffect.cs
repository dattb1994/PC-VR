using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class SwitchSceneBlurEffect : Singleton<SwitchSceneBlurEffect>
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    public float fadeDuration = 1f;
    private Tween fadeTween;
    private Tween colorTween;

    public override void Awake()
    {
        base.Awake();
        if (volume.profile.TryGet(out colorAdjustments) == false)
        {
            Debug.LogError("Color Adjustments not found in Volume Profile!");
        }
    }

    /// <summary>
    /// Hiện ra lớp phủ
    /// </summary>
    public void ShowBlur()
    {
        fadeTween?.Kill();

        colorTween = DOTween.To(
            () => colorAdjustments.colorFilter.value,
            x => colorAdjustments.colorFilter.value = x,
            Color.black,
            fadeDuration
        );
    }

    /// <summary>
    /// Ẩn màn che
    /// </summary>
    public void HideBlur()
    {
        fadeTween?.Kill();
        colorTween = DOTween.To(
            () => colorAdjustments.colorFilter.value,
            x => colorAdjustments.colorFilter.value = x,
            Color.white,
            fadeDuration
        );
    }

    /// <summary>
    /// Hiện ra lớp phủ
    /// </summary>
    public void FadeColorFilter(Color targetColor)
    {
        colorTween?.Kill();
        colorTween = DOTween.To(
            () => colorAdjustments.colorFilter.value,
            x => colorAdjustments.colorFilter.value = x,
            targetColor,
            fadeDuration
        );
    }
    public void ForceShowBlur()
    { 
        colorAdjustments.colorFilter.value = Color.black;
    }
    /// <summary>
    /// Ẩn màn che
    /// </summary>
    public void ForceHideBlur()
    {
        colorAdjustments.colorFilter.value = Color.white;
    }
}
