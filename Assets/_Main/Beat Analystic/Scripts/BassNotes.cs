using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BassNotes
{
    public List<float> beats;
    public float? bpm;  // Nullable float
    public string audio_file;
    public DetectionSettings detection_settings;

    [System.Serializable]
    public class DetectionSettings
    {
        public float threshold;
        public float min_interval;
        public int frame_length;
    }
}
