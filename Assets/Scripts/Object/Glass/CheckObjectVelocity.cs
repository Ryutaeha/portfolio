using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObjectVelocity : MonoBehaviour
{
    private Break destroy;
    [SerializeField] private float distroyVelocity; //destroy인거 알지만 냅둠 오타임

    private Vector3 beforePosition;
    private bool canDestroy;
    private bool onlyDestroy = false;
    private DestroyedObject DO;
    private void Awake()
    {
        destroy= transform.parent.GetComponent<Break>();
        beforePosition = transform.position;
        DO = GetComponent<DestroyedObject>();
    }

    private void Update()
    {
        if ((beforePosition - transform.position).magnitude > 2) //물체의 이동값이 깨지기 충분 할 때
        {
            canDestroy = true;
        }
        else
        {
            canDestroy = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (DO.Type == ObjectPropertyType.DESTROY)
        {
            if (!onlyDestroy)
            {
                if (canDestroy) //유리 자체의 속도가 클 때
                {
                    destroy.Destroying();
                    onlyDestroy = true;
                }

                if (collision.gameObject.layer == LayerMask.NameToLayer("Interactive")) //물체를 유리에 던졌을 때
                {
                    if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        if (rb.velocity.magnitude > distroyVelocity)
                        {
                            DestroyGlass();
                            onlyDestroy = true;
                        }
                    }
                    //if (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > distroyVelocity)
                    //{
                    //}
                }
            }
        }
    }

    public void DestroyGlass()
    {
        destroy.Destroying();
    }
}
