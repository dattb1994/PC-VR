using UnityEngine;

[CreateAssetMenu(menuName = "Config/Smoke")]
public class SmokeConfig : ScriptableObject
{
    public int gameplayQuality = 0;
    [Header("Smoke")]
    public float colorIntensity;
    public float alpha;
    public float vfxMultiplier;
    public float particleSize;
    public Color smokeColor;

    [Header("Ember")]
    public float embersVFXMultiplier;
    public float embersBurstVFXMultiplier;
    public float embersParticleSize;

    [Header("Fire")]
    [Tooltip("Độ phức tạp của ngọn lửa")]
    public int meshFireCount;
    public float flameLength;
    public float flameVFXMultiplier;
    public float flameParticleSize;
}
