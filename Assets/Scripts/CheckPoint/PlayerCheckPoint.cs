using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    [SerializeField] private Vector3 lastCheckPoint;
    [SerializeField] private LayerMask deadZone;
    CharacterController characterController;
    public Vector3 LastCheckPoint { get { return lastCheckPoint; } set { lastCheckPoint = value; } }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    public void CheckPoint(Vector3 checkPoint)
    {
        lastCheckPoint = checkPoint;
        Debug.Log(lastCheckPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((deadZone.value & (1 << other.gameObject.layer)) != 0)
        {
            ReSpawn();
        }
    }
    public void ReSpawn()
    {
        characterController.enabled = false;
        gameObject.transform.position = lastCheckPoint;
        characterController.enabled = true;
    }
}
