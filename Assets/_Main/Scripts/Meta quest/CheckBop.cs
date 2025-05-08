using UnityEngine;

public class CheckBop : MonoBehaviour
{
    public HoseParticle hoseSpinkler;
    public GameObject obj;
    private void Start()
    {
        obj.SetActive(false);
    }
    private void Update()
    {
        if (hoseSpinkler.Spinkler > 0)
        { 
            gameObject.SetActive(false);
        }
    }
}
