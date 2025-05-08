using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CoalParticle : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject coal;

    public float timeSmoke = 10;

    private IEnumerator Start()
    {
        var renCoal = coal.GetComponent<Renderer>();

        particle.gameObject.SetActive(true);
        //coal.SetActive(false);

        float time = timeSmoke;
        var emit = particle.emission;
        float originEmitCount = emit.rateOverTimeMultiplier;

        float a_color = 1;
        var coalColor = renCoal.material.color;
        while (time > 0)
        {
            time -= Time.deltaTime;
            emit.rateOverTime = originEmitCount * (timeSmoke / 10);
            renCoal.material.color = coalColor * new Color(1, 1, 1, a_color * (1 - time / timeSmoke));
            yield return null;
        }

        particle.gameObject.SetActive(false);
        //coal.SetActive(true);

    }
}
