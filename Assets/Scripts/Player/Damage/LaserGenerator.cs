using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    GameObject go;
    void Start()
    {
        StartCoroutine(CoGenerateLaser());
    }

    IEnumerator CoGenerateLaser()
    {
        while (true)
        {
            go = Main.Resource.Instantiate("MovingLaser.prefab", transform, true, true);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("플레이어 닿음");
        }
    }
}
