using System.Collections.Generic;

[System.Serializable]
public class DrumNotes
{
    public List<DrumBeat> beats;
    public float bpm;
    public AnalysisSettings analysis_settings;

    [System.Serializable]
    public class AnalysisSettings
    {
        public int sample_rate;
        public int frame_length;
        public int hop_length;
        public float threshold;
        public float min_interval;
        public int[] kick_freq_range;
        public int[] snare_freq_range;
    }
}
[System.Serializable]
public class DrumBeat
{
    public float time;
    public string type; // "kick" or "snare"
    public float intensity;
    public bool accent;
    public float spectral_flux;
}