using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private GameObject Player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player = other.gameObject;

            if (Player.transform.TryGetComponent<IDamageable>(out IDamageable comp))
            {
                Debug.Log("Player공격");
                comp.TakeDamage(120);
            }
        }
    }
}
