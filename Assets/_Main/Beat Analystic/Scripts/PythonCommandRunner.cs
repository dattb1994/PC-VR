using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PythonCommandRunner : MonoBehaviour
{
    public string _audio;
    public string folder;
    [Header("Result")]
    public BassNotes bassNotes;
    public DrumNotes drumNotes;

    [Header("Command prompt")]
    public string commandBass;
    public string commandDrum;
    public string folderCMD;



    public void GenCommandDrum()
    {
        print("GenCommandDrum");
        /// python drum.py audio.mp3 strong_beats.json --threshold 0.3
        commandDrum = $"python drum.py {_audio}.mp3 {_audio}_drum.json --threshold 0.3";
    }
    public void GenCommandBass()
    {
        print("GenCommandBass");
        /// python drum.py audio.mp3 strong_beats.json --threshold 0.3
        commandBass = $"python bass.py {_audio}.mp3 {_audio}_bass.json --threshold 0.3";
    }
    public void GenFolder()
    {
        print("GenFolder");
    }

    internal void LoadBass()
    {
        string file = $"{_audio}_bass.json";
        string path = Path.Combine(folder, file);
        string json = System.IO.File.ReadAllText(path);
        bassNotes = JsonUtility.FromJson<BassNotes>(json);
        print($"LoadBass {json}");
    }

    internal void LoadDrum()
    {
        string file = $"{_audio}_drum.json";
        string path = Path.Combine(folder, file);
        string json = System.IO.File.ReadAllText(path);
        drumNotes = JsonUtility.FromJson<DrumNotes>(json);
        print($"LoadDrum {json}");
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PythonCommandRunner))]
public class PythonCommandRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PythonCommandRunner script = (PythonCommandRunner)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Gen command bass"))
        {
            script.GenCommandBass();
        }
        if (GUILayout.Button("Gen command drum"))
        {
            script.GenCommandDrum();
        }
        if (GUILayout.Button("Chọn Thư Mục"))
        {
            string path = EditorUtility.OpenFolderPanel("Chọn Thư Mục", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                script.folderCMD = $"cd /d {path}";
                script.folder = path;
                Debug.Log("Đã chọn thư mục: " + path);
            }
        }
        if (GUILayout.Button("Load bass"))
        {
            script.LoadBass();
        }
        if (GUILayout.Button("Load drum"))
        {
            script.LoadDrum();
        }
    }
}
#endif