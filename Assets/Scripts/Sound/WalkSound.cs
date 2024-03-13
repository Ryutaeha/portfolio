using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    private float lastFootstepTime;
    private float beforePositionX;
    private float beforePositionZ;
    private string SoundName;
    private float stepVolume;

    private PlayerEventController playerEventController;
    private AudioSource audioSource;
    private AudioClip stepClip;

    private bool isPlayerMove;

    void Start()
    {
        playerEventController = GetComponentInParent<PlayerEventController>();
        audioSource = GetComponent<AudioSource>();
        beforePositionX = transform.position.x; // 현재 위치값(x) 저장
        beforePositionZ = transform.position.z; // 현재 위치값(z) 저장
    }

    void Update()
    {

        if (Mathf.Abs(transform.position.x - beforePositionX) > 0.01f || Mathf.Abs(transform.position.z - beforePositionZ) > 0.01f)
        {
            isPlayerMove = true;
        }
        else
        {
            isPlayerMove = false;
        }

        FootStepSoundOutput();
    }

    private void FootStepSoundOutput()
    {
        if (playerEventController.IsGrounded() && isPlayerMove) //땅에 닿았는 상태인가, 위치가 변해있는가-> 걸었는가
        {
            lastFootstepTime += Time.unscaledDeltaTime;
            if (lastFootstepTime > 0.5f) //걷기 딜레이 0.5초
            {

                if (playerEventController.Jump)
                {
                    stepClip = Main.Sound.FindAudioClip("JumpSound");
                    audioSource.volume = 0.3f;
                    audioSource.clip = stepClip;
                    audioSource.Play();
                }
                else
                {
                    FindStepSoundClip();

                    if (!playerEventController.isSit)
                        stepVolume = 0.7f; //서 있는 경우
                    else
                        stepVolume = 0.3f; //앉은 경우

                    audioSource.volume = stepVolume;
                    audioSource.clip = stepClip;
                    audioSource.Play();
                }

                lastFootstepTime = 0;
            }
            beforePositionX = transform.position.x;
            beforePositionZ = transform.position.z;
        }
    }

    private void FindStepSoundClip()
    {
        int rand = Random.Range(0, 2); //발소리 2가지 중 1개 랜덤
        if (rand == 0)
        {
            stepClip = Main.Sound.FindAudioClip("FootStep0");
        }
        else if (rand == 1)
        {
            stepClip = Main.Sound.FindAudioClip("FootStep1");
        }
    }
}
