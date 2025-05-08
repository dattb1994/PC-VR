using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFan : MonoBehaviour
{
    public bool isTrackingPlayer = false;
    AudioSource audioSource;
    public AudioClip shakingClip, fallingClip;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Active()
    {
        GetComponent<Animator>().SetTrigger("active");
        audioSource.loop = true;
        audioSource.clip = shakingClip;
        audioSource.Play();
        if (GameSetting.level < 6)
            ProgressCtrl.Instance.warningSapCauKienUI.SetActive(true);
    }

    public void FollingDown()
    {
        if (GameSetting.level < 6)
            ProgressCtrl.Instance.warningSapCauKienUI.SetActive(false);
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = fallingClip;
        audioSource.Play();
        GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        isTrackingPlayer = true;
        Invoke(nameof(CancelTrackingPlayer), 1.5f);
    }
    void CancelTrackingPlayer()
    {
        isTrackingPlayer = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTrackingPlayer)
            return;

        if (other.gameObject.CompareTag("MainCamera"))
        {
            ScoreMechanic.Instance.AddError("Building collapse.", .5f);
            isTrackingPlayer = false;
        }
    }
}
