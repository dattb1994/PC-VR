using UnityEngine;
using UnityEngine.Events;

public class ThuLang : MonoBehaviour
{
    HoseParticle hose;

    public bool isTrying = false;

    public UnityAction OnStried;

    public float threshold = 3;

    private void Start()
    {
        hose = GetComponent<FireSteering>().Hose;
        hose.OnToggleChanged += OnToggleChanged;
    }

    public float countTime = 0;
    public float count = 0;

    private void Update()
    {
        if (!isTrying)
            return;

        countTime -= Time.deltaTime;
        if (countTime < 0)
            count = 0;
    }
    void OnToggleChanged(bool value)
    {
        if (!isTrying)
            return;

        if (count < threshold)
        {
            count++;
            countTime = 2;
        }
        else
        {
            OnStried();
        }
    }
}
