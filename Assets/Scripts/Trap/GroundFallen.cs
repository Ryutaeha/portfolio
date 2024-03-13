using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFallen : MonoBehaviour
{
    public GameObject Plate1;
    public GameObject Plate2;
    public GameObject BrokenFloor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Plate1.SetActive(false);
            Plate2.SetActive(false);
            BrokenFloor.SetActive(true);
        }
    }
}
