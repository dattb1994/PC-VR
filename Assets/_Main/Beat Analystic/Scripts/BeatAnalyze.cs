using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Linq;

public class BeatAnalyze : MonoBehaviour
{
    public string pythonPath = "C:\\Users\\YourName\\AppData\\Local\\Programs\\Python\\Python39\\python.exe"; // Đường dẫn Python
    public string command = "drum.py audio.mp3 strong_beats.json --threshold 0.3"; // Lệnh mặc định
    public string folder = ""; // Thư mục mặc định

    [ContextMenu("Func 1")]
    public void ExecuteCommand()
    {
        RunCommand(command, folder);
    }

    private void RunCommand(string command, string folder)
    {
        string[] commandParts = command.Split(' '); // Tách các tham số
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = pythonPath, // Chạy Python trực tiếp
            Arguments = $"{commandParts[0]} \"{folder}\\{commandParts[1]}\" {string.Join(" ", commandParts.Skip(2))}",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process { StartInfo = psi };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError("Command Error: " + error);
        }
        else
        {
            UnityEngine.Debug.Log("Command Output: " + output);
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(BeatAnalyze))]
public class BeatAnalyzeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BeatAnalyze script = (BeatAnalyze)target;
        if (GUILayout.Button("Func 1"))
        {
            script.ExecuteCommand();
        }
    }
}
#endif
