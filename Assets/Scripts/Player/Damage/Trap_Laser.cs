using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Laser : MonoBehaviour
{
    [SerializeField] float _rayDistance = 5.0f;
    [SerializeField] private LayerMask _playerLayer;
    public bool isMoving;
    public float Speed = 10f;
    private RaycastHit _hit;
    private AudioSource audioSource;
    private AudioClip laser;
    private void OnEnable()
    {
        if (isMoving)
        {
            StopAllCoroutines();
            StartCoroutine(CoReturnPool());
        }
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        laser = Main.Sound.FindAudioClip("LaserDead");
        if (laser != null)
        {
            Debug.Log(laser.name);
        }
        else
        {
            Debug.Log("null");
        }
        audioSource.clip = laser;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.up * Speed * Time.deltaTime;
        }
        if (Physics.Raycast(transform.position, transform.up, out _hit, _rayDistance, _playerLayer))
        {
            Debug.Log("hit");
            if(_hit.transform.TryGetComponent<IDamageable>(out IDamageable comp))
            {
                audioSource.Play();
                comp.TakeDamage(120);
            }
        }
    }
    IEnumerator CoReturnPool()
    {
        yield return new WaitForSeconds(3.0f);
        Main.Resource.Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * _rayDistance);
    }
}
