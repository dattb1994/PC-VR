using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseParticleWater : HoseParticle
{
    public override void HandleSpinklerChanged()
    {
        var emission = vfx.emission;

        if (Spinkler <=0 )
        {
            emission.rateOverTime = 0;
        }

        emission.rateOverTime = Spinkler + 50;
    }
}
// 1 --  100
// 50--  150
