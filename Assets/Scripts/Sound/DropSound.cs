using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            Main.Sound.PlaySFX("DropSound", transform.position, 0.4f);
        }
    }
}
