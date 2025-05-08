using TigerForge;
using UnityEngine;

public static class GameSetting
{
    public static TypeEquip typeEquipment = 0;
    public static int typeEnv = 0;
    public static int indexEnv = 0;
    public static int level = 1;
    private static float sprinkler = 0;// spinkler for Binh from server
    public static float Sprinkler
    {
        set
        {
            sprinkler = value;
            EventManager.EmitEvent(EventKey.onSprinklerChanged.ToString());
        }
        get
        {
            return sprinkler;
        }
    }

    public static Vector2 clampTimeBurnLength = new Vector2(300, 600);
    internal static string sceneToLoad = "Menu Van phong";
    public static float effectIntensity
    {
        set {
            PlayerPrefs.SetFloat("effectIntensity", value);
            EventManager.EmitEvent(EventKey.onEffectIntensityChanged.ToString());
        }
        get
        {
            if (PlayerPrefs.HasKey("effectIntensity"))
                return PlayerPrefs.GetFloat("effectIntensity");
            else
                return 1;
        }
    }
}