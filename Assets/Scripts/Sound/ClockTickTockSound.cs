using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockTickTockSound : MonoBehaviour
{
    private AudioSource clockSound;
    private AudioClip clockClip;

    private void Awake()
    {
        clockSound = GetComponent<AudioSource>();
        clockClip = Main.Sound.FindAudioClip("TickTock");
        clockSound.clip = clockClip;
        //해결방법 2
        clockClip.LoadAudioData();

        //해결방법 1
        //AudioClip clip = Main.Resource.GetResource<AudioClip>("TickTock.mp3");
        //clip.LoadAudioData();
        //clockSound.clip = clip;

        //처음 재생시 발생하는 프레임 드랍 방지용 임시방편
        //clockSound.volume = 0f;
        //clockSound.Play();
        //clockSound.Stop();
        //clockSound.volume = 0.6f;
    }

    public void PlayTickTock()
    {
        StartCoroutine(PlayTickTockWithDelay());
    }

    private IEnumerator PlayTickTockWithDelay()
    {
        yield return new WaitForSeconds(0.7f); // 0.1초 지연
        clockSound.Play();
    }

    public void StopTickTock()
    {
        clockSound.Stop();
    }
    private void OnDestroy()
    {
        clockSound.Stop();
    }
}
