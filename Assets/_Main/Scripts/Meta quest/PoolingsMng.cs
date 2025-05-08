using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zef;
public class PoolingsMng : Singleton<PoolingsMng>
{
    public List<Zef.Pooling> items;
    public GameObject Play(int indexPooling)
    {
        var audioSource = items[indexPooling].Get().GetComponent<AudioSource>();
        audioSource.gameObject.SetActive(true);
        DOVirtual.DelayedCall(audioSource.clip.length, () =>
        {
           audioSource.gameObject.SetActive(false);
        });
        return items[indexPooling].Get();
    }

    internal void LoadPooling()
    {
        items.Clear();
        foreach (var item in GetComponentsInChildren<Pooling>())
        {
            items.Add(item);
        }
    }
}
//[CustomEditor(typeof(PoolingsMng))]
//public class PoolingsMngEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); // Vẽ Inspector mặc định

//        PoolingsMng myScript = (PoolingsMng)target;

//        GUILayout.BeginHorizontal();
//        GUILayout.FlexibleSpace(); // Đẩy nút về giữa
//        if (GUILayout.Button("Reset pool", GUILayout.Width(120), GUILayout.Height(25)))
//        {
//            myScript.LoadPooling();
//        }
//        GUILayout.FlexibleSpace(); // Đẩy nút về giữa
//        GUILayout.EndHorizontal();
//    }
//}