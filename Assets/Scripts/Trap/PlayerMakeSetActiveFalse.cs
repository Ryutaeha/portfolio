using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMakeSetActiveFalse : MonoBehaviour
{
    public GameObject[] GameObjects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            foreach(GameObject go in GameObjects)
            {
                go.SetActive(false);
            }            
        }
    }
}
