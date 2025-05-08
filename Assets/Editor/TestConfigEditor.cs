using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[CustomEditor(typeof(TestModeConfig))]
public class TestConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TestModeConfig test = (TestModeConfig)target;
        if (GUILayout.Button("Compress texture"))
        {
            CompressTexture(test.textureSize);
        }
        if (GUILayout.Button("Build"))
        {
            BuildGame();
        }
    }
    public void CompressTexture(int size)
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture");

        int count = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                bool changed = false;

                if (importer.maxTextureSize > size)
                {
                    importer.maxTextureSize = size;
                    changed = true;
                }

                if (changed)
                {
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    count++;
                    Debug.Log($"Set max size {size}: {path}");
                }
            }
        }

        Debug.Log($"✅ Done! {count} textures were updated to max size {size}.");
    }
    private void BuildGame()
    {
        //MenuManager menuManager = (MenuManager)target;
        string namefile = $"{PlayerSettings.productName} {PlayerSettings.bundleVersion}".Replace(":", " ");

        // Mở hộp thoại chọn nơi lưu file (ví dụ build cho Windows .exe)
        string path = EditorUtility.SaveFilePanel(
            "Chọn nơi lưu build",
            "",
            $"{namefile}",
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
}
