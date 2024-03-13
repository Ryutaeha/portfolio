using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    [SerializeField] private int narrationIndex; // 재생할 나레이션의 인덱스
    [SerializeField] private int narrationCount; // 재생할 나레이션의 인덱스

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Main.Game.HandleNarrationPlaying(narrationIndex, narrationCount);
            Destroy(this.gameObject);
        }
    }
}
