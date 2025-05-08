using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameConfig", menuName = "PCCC/FlameConfig")]
public class FlameConfig : ScriptableObject
{
    [Tooltip("thời gian đám cháy tự cháy hết, từ x -> y phụ thuộc level được thiết lập")]
    public Vector2 clampTimeBurnLength = new Vector2(100, 200); //(300, 600);

    public SmokeConfig smokeConfig;
    public GameObject customFireSFX;
}
