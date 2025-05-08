using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TestModeConfig", menuName = "PCCC/TestModeConfig")]
public class TestModeConfig : ScriptableObject
{
    public bool isTestMode;
    public bool useEquipCms;
    public int typeEnv;
    public int indexEnv;
    public TypeEquip typeEquipment;
    public int level;
    public bool showWall;
    public int textureSize;
}
