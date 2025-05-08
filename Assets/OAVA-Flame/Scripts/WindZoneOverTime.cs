using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneOverTime : MonoBehaviour
{
    WindZone windZone;
    public float step = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        windZone = GetComponent<WindZone>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetValueWindZone(int value)
    {
        StartCoroutine(IncreaseWindZone(value));
    }
    IEnumerator IncreaseWindZone(int value)
    {
        while (windZone.windMain < value)
        {

            windZone.windMain++;
            yield return new WaitForSeconds(step);
        }
        windZone.windMain = value;
    }
}
