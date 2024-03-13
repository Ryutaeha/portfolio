using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectViewActive : MonoBehaviour
{
    public GameObject ActiveObject;

    private void Start()
    {
        ActiveObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!ActiveObject.activeSelf)
            {
                ActiveObject.SetActive(true);
            }
        }
    }
}
