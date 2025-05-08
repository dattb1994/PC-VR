using UnityEngine;

public static class GameConfig
{
    private static FlameConfig flameConfig;
    public static FlameConfig @FlameConfig
    {
        get
        {
            if (flameConfig == null)
            {
                flameConfig = Resources.Load<FlameConfig>("FlameConfig");
            }

            return flameConfig;
        }
    }

    private static TestModeConfig testConfig;
    public static TestModeConfig TestConfig
    {
        get
        {
            if (testConfig == null)
            {
                testConfig = Resources.Load<TestModeConfig>("TestModeConfig");
            }

            return testConfig;
        }
    }

    public static IPConfig ipConfig;
    public static IPConfig @IpConfig
    {
        get
        {
            if (ipConfig == null)
            {
                string path = Application.streamingAssetsPath + "/ipconfig.txt";
                var i = JsonUtility.FromJson<IPConfig>(System.IO.File.ReadAllText(path));
                ipConfig = new IPConfig(i.IP_CMS, i.IP_Eqipment, i.dev_lang, i.dev_binh_co2, i.dev_binh_bot);
            }
            return ipConfig;
            
        }
    }

}
[System.Serializable]
public class IPConfig
{
    public string IP_CMS;
    public string IP_Eqipment;
    public string dev_lang;
    public string dev_binh_co2;
    public string dev_binh_bot;
    public IPConfig(string iP_CMS, string iP_Eqipment, string dev_lang, string dev_binh_co2, string dev_binh_bot)
    {
        IP_CMS = iP_CMS;
        IP_Eqipment = iP_Eqipment;
        this.dev_lang = dev_lang;
        this.dev_binh_co2 = dev_binh_co2;
        this.dev_binh_bot = dev_binh_bot;
    }
}