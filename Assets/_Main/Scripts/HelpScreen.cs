using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HelpScreen : MonoBehaviour
{
    public List<VideoClip> videoClips;
    public VideoPlayer player;

    public GameObject main, videoPlayer;
    private void OnEnable()
    {
        Main();
    }
    public void PlayVideo(int index)
    {
        player.clip = videoClips[index];
        main.SetActive(false);
        videoPlayer.SetActive(true);
        player.Play();
    }
    public void Main()
    {
        player.Stop();
        main.SetActive(true);
        videoPlayer.SetActive(false);
    }
}
