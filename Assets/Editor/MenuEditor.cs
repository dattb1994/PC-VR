using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MenuManager))]
public class MenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MenuManager menuManager = (MenuManager)target;
        if (GUILayout.Button("Change Setting"))
        {
            string[] nameRoom = new string[] { "Fire Fighting: Office", "Fire Fighting: Apartment Living Room", "Fire Fighting: Living Room with Garage", "Fire Fighting: Kitchen", "Fire Fighting: Bedroom", "Fire Fighting: Warehouse" };
            menuManager.txtNameGame.text = nameRoom[(int)menuManager.roomSetting.typeEnv];
            string productName = nameRoom[(int)menuManager.roomSetting.typeEnv];
            PlayerSettings.productName = productName;
            string cleanName = productName.Replace(":", "").Replace(" ", "");

            // Cập nhật thông tin cho build Windows

            // Đặt bundle ID (Application Identifier)
            string bundleId = $"com.VRTechJSC.{cleanName}";
            if (menuManager.roomSetting.typeEnv == TypeRoom.living_room_with_garage)
                bundleId = "com.VRTechJSC.FireFighting.LivingRoomwithGarage";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, bundleId);
            PlayerSettings.applicationIdentifier = bundleId;

            BuildSetting buildSetting = new BuildSetting();
            buildSetting.scenesMain = new List<EditorBuildSettingsScene>();
            buildSetting.scenesMain.Add(new EditorBuildSettingsScene("Assets/_Main/Scenes/LoadData.unity", true));
            buildSetting.scenesMain.Add(new EditorBuildSettingsScene("Assets/_Main/Scenes/Menu Van phong.unity", true));
            buildSetting.scenesMain.Add(new EditorBuildSettingsScene("Assets/_Main/Scenes/Main.unity", true));

            string[] folder = new string[] { "Office", "Apartment living room", "Fire Fighting Living Room with Garage", "Fire Fighting Kitchen", "Fire Fighting Bedroom", "Fire Fighting Warehouse" };

            buildSetting.currentScenes = new List<EditorBuildSettingsScene>();
            for (int i = 0; i < menuManager.roomSetting.maxRoom; i++)
            {
                int n = i + 1;
                string scene = $"Assets/_Main/Scenes/{folder[(int)menuManager.roomSetting.typeEnv]}/{n}.unity";
                buildSetting.currentScenes.Add(new EditorBuildSettingsScene(scene, true));
            }

            EditorBuildSettings.scenes = buildSetting.scenes.ToArray();

            PlayerSettings.bundleVersion = menuManager.roomSetting.version;
            PlayerSettings.Android.bundleVersionCode = menuManager.roomSetting.bundleVersionCode;

            SetKeystoreSettings();
        }
        if (GUILayout.Button("Build"))
        {
            BuildGame();
        }
    }
    private void BuildGame()
    {
        MenuManager menuManager = (MenuManager)target;
        // Mở hộp thoại chọn nơi lưu file (ví dụ build cho Windows .exe)
        string path = EditorUtility.SaveFilePanel(
            "Chọn nơi lưu build",
            "",
            $"{menuManager.roomSetting.typeEnv} {menuManager.roomSetting.version}",
            "apk" // hoặc "apk" nếu là Android
        );

        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("❌ Hủy build vì chưa chọn đường dẫn.");
            return;
        }

        // Tự động lấy scene từ Build Settings
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = path,
            target = BuildTarget.Android, // đổi platform ở đây nếu cần
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("✅ Build thành công: " + summary.totalSize + " bytes");
            EditorUtility.RevealInFinder(path); // Mở thư mục sau khi build xong
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogError("❌ Build thất bại");
        }
    }

    public void SetKeystoreSettings()
    {
        MenuManager menuManager = (MenuManager)target;
        // Bật custom keystore
        PlayerSettings.Android.useCustomKeystore = true;

        // Đường dẫn tới keystore file (.keystore hoặc .jks)
        string user = "";
        switch (menuManager.roomSetting.typeEnv)
        { 
            case TypeRoom.office:
                user = "Fire Fighting Office";
                break;
            case TypeRoom.apartment_living_room:
                user = "Apartment Living Room";
                break;
            case TypeRoom.living_room_with_garage:
                user = "Fire Fighting Living Room with Garage";
                break;
            case TypeRoom.kitchen:
                user = "Fire Fighting Kitchen";
                break;
            case TypeRoom.bedroom:
                user = "Fire Fighting Bedroom";
                break;
            case TypeRoom.warehouse:
                user = "Fire Fighting Warehouse";
                break;

        }
        PlayerSettings.Android.keystoreName = $"Assets/Keystore/{user}.keystore"; // hoặc "Assets/Keystore/user.keystore"

        // Mật khẩu cho keystore và key alias
        PlayerSettings.Android.keystorePass = "123456789";
        PlayerSettings.Android.keyaliasName = "1";
        PlayerSettings.Android.keyaliasPass = "123456789";

        Debug.Log("✅ Keystore đã được cấu hình!");
    }
    [MenuItem("Build Tools/Log Media Assets Used In Scenes")]
    public static void LogAssetsUsedInBuildScenes()
    {
        string[] scenePaths = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        HashSet<string> usedAssetPaths = new HashSet<string>();

        foreach (string scenePath in scenePaths)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var rootObjects = scene.GetRootGameObjects();
            var dependencies = EditorUtility.CollectDependencies(rootObjects);

            foreach (var obj in dependencies)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(assetPath) && assetPath.StartsWith("Assets/"))
                {
                    usedAssetPaths.Add(assetPath);
                }
            }
        }

        // Lọc theo loại file media
        string[] allowedExtensions = {
            ".mp4", ".mov", ".webm",             // video
            ".png", ".jpg", ".jpeg", ".tga",
            ".psd", ".tiff", ".bmp",             // image
            ".mp3", ".wav", ".ogg", ".aiff",     // audio
            ".fbx", ".obj", ".dae", ".blend"     // model
        };

        var filteredAssets = usedAssetPaths
            .Where(path => allowedExtensions.Contains(Path.GetExtension(path).ToLower()))
            .ToList();

        string logPath = Path.Combine(Application.dataPath, "../log_build.txt");

        using (StreamWriter writer = new StreamWriter(logPath))
        {
            writer.WriteLine("🎯 Media Assets Used In Build Scenes:");
            writer.WriteLine("======================================\n");

            foreach (var path in filteredAssets.OrderByDescending(p => new FileInfo(p).Length))
            {
                long size = new FileInfo(path).Length;
                string sizeMB = (size / (1024f * 1024f)).ToString("F2") + " MB";
                writer.WriteLine($"{sizeMB.PadLeft(8)}  -  {path}");
            }
        }

        Debug.Log($"✅ Đã ghi log vào: {logPath}");
    }
}
[System.Serializable]
public struct BuildSetting
{
    public List<EditorBuildSettingsScene> scenesMain;
    public List<EditorBuildSettingsScene> currentScenes;
    public List<EditorBuildSettingsScene> scenes
    {
        get
        {
            List<EditorBuildSettingsScene> result = new List<EditorBuildSettingsScene>();
            foreach (var scene in scenesMain)
            {
                if (scene.enabled)
                {
                    result.Add(scene);
                }
            }
            foreach (var scene in currentScenes)
            {
                if (scene.enabled)
                {
                    result.Add(scene);
                }
            }
            return result;
        }
    }
}
