using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFallingTrap : MonoBehaviour
{
    private float waitTime;
    private AudioSource audioSource;
    private AudioClip audioClip;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClip = Main.Sound.FindAudioClip("DropSound");
        audioSource.clip = audioClip;
        waitTime = Random.Range(0f, 2f);
        StartCoroutine(StartFalling());
    }
    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(waitTime);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(new Vector3(transform.position.x, transform.position.y - 7.8f, transform.position.z), 1f, 1, 0.5f).SetEase(Ease.InExpo));
        sequence.AppendInterval(1f); // 1초 동안 대기
        sequence.Append(transform.DOMoveY(transform.position.y, 3f)).SetEase(Ease.Linear);
        sequence.SetLoops(-1);
        sequence.Play();

        // Tweener 시작
        sequence.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(120);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            audioSource.Play();
        }
    }
}