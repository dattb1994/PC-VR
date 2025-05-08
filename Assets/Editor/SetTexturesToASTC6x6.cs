using UnityEngine;
using UnityEditor;

public class SetTexturesToASTC6x6 : EditorWindow
{
    [MenuItem("Tools/Set All Textures Max Size to 512")]
    public static void SetMaxSizeTo512()
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

                if (importer.maxTextureSize > 128)
                {
                    importer.maxTextureSize = 128;
                    changed = true;
                }

                if (changed)
                {
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    count++;
                    Debug.Log($"Set max size 512: {path}");
                }
            }
        }

        Debug.Log($"✅ Done! {count} textures were updated to max size 512.");
    }
}
