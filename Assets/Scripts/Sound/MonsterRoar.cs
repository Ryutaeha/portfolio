using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoar : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip roarClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        roarClip = Main.Sound.FindAudioClip("MonsterAttack");
    }

    public void MonsterAttackSound()
    {
        audioSource.PlayOneShot(roarClip);
    }



}
