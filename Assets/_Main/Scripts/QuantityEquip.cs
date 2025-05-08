using Ignis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuantityEquip : MonoBehaviour
{
    public float maxTimeUse_m = 15;
    float current;
    float delta = 1;
    float frequency = .5f;
    HoseParticle hoseParticle;
    float countTime = 0;
    void Start()
    {
        current = 12000 * maxTimeUse_m;
        hoseParticle = GetComponent<HoseParticle>();
    }
    private void Update()
    {
        if(hoseParticle.IsOn && hoseParticle.Spinkler > 0)
        {
            if(countTime > 0)
            {
                countTime -= Time.deltaTime;
            }
            else
            {
                if (current > 0)
                {
                    current -= hoseParticle.Spinkler * delta;
                }
                else
                    hoseParticle.IsOn = false;
                countTime = frequency;
            }
        }
    }
}
