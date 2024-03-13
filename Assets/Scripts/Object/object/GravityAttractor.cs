using UnityEngine;
using System.Collections.Generic;

public class GravityAttractor : MonoBehaviour
{
    public float gravityForce = -10f;
    public float raidous = 10f;

    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, raidous);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = transform.position - rb.position;
                rb.AddForce(direction.normalized * gravityForce * Time.fixedDeltaTime* (1 /Time.timeScale));
            }
        }
    }
}
