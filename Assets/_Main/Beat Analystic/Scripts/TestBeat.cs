using System.Collections.Generic;
using UnityEngine;

public class TestBeat : MonoBehaviour
{
    public string beats;
    public string drumJson;
    public BassNotes beatNotes;
    public List<float> beatTimes = new List<float>();


    public DrumNotes drumNotes;
    private void Start()
    {
        beatNotes = JsonUtility.FromJson<BassNotes>(beats);

        drumNotes = JsonUtility.FromJson<DrumNotes>(drumJson);
    }
}
