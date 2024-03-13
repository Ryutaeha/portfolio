using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    //private float volume;
    //private float tmpVolume;
    //private bool isPlayerEnter;
    [SerializeField] private string AfterBgmName;

    //private void Awake()
    //{
    //    CachingVolume();
    //    tmpVolume = volume;
    //}
    //private void Update()
    //{
    //    if (volume != PlayerPrefs.GetFloat("MusicVolume"))
    //    {
    //        CachingVolume();
    //    }

    //    if (isPlayerEnter)
    //    {
    //        if (volume > -40)
    //        {
    //            float tmpV = volume;
    //            tmpV -= 1f;
    //            PlayerPrefs.SetFloat("MusicVolume", tmpV);
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetFloat("MusicVolume", tmpVolume);
    //            CachingVolume();
    //        }
    //    }
    //}
    //private void CachingVolume()
    //{
    //    volume = PlayerPrefs.GetFloat("MusicVolume");
    //    Main.Sound.SetVolume("Music", volume);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerEnter = true;
            Main.Sound.StopBGM();
            Main.Sound.PlayBGM(AfterBgmName, 0.5f);
            Destroy(this);
        }
    }
}
