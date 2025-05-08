using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Zef
{
    public class Pooling : MonoBehaviour
    {
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;

        void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
        public GameObject Get()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            return null;
        }

        internal void CreatPrefab()
        {
            GameObject prefab = new GameObject(gameObject.name);
            prefab.transform.SetParent(transform);
            prefab.AddComponent<AudioSource>();
            prefab.SetActive(false);
            objectToPool = prefab;
        }
    }
    //[CustomEditor(typeof(Pooling))]
    //public class PoolingEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector(); // Vẽ Inspector mặc định

    //        Pooling myScript = (Pooling)target;

    //        GUILayout.BeginHorizontal();
    //        GUILayout.FlexibleSpace(); // Đẩy nút về giữa
    //        if (GUILayout.Button("Creat prefab", GUILayout.Width(120), GUILayout.Height(25)))
    //        {
    //            myScript.CreatPrefab();
    //        }
    //        GUILayout.FlexibleSpace(); // Đẩy nút về giữa
    //        GUILayout.EndHorizontal();
    //    }
    //}
}

