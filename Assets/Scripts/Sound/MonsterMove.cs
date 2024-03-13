using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clip;
    private float audioClipLength;
    private float beforePositionX;
    private float beforePositionZ;

    private PlayerNearby monster;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        beforePositionX = transform.position.x; // 현재 위치값(x) 저장
        beforePositionZ = transform.position.z; // 현재 위치값(z) 저장
        clip = Main.Sound.FindAudioClip("WetStep");

        monster = GetComponentInParent<PlayerNearby>();

        audioSource.clip = clip;
        Debug.Log(clip.name);
    }

    private void Start() 
    {
        audioSource.Play();
        audioSource.Pause(); // 첫 추격때 소리 안나는 문제 임시방편
    }

    private void Update()
    {
        bool isThisMove;

        if (Mathf.Abs(transform.position.x - beforePositionX) > 0.001f || Mathf.Abs(transform.position.z - beforePositionZ) > 0.001f)
        {
            isThisMove = true;
        }
        else
        {
            isThisMove = false;
        }

        if (isThisMove && !monster.isAttack) //걷고있고 공격중이 아닐때
        {
            if (audioClipLength <= 0)
            {
                audioSource.Play();
                audioClipLength = 2.7f;
            }
            audioClipLength -= Time.unscaledDeltaTime;

            beforePositionX = transform.position.x;
            beforePositionZ = transform.position.z;
        }
        else
        {
            audioSource.Pause();
            audioClipLength = 0.1f;
        }
    }
}
