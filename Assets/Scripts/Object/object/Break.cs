using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public GameObject Prefab;
    public float breakForce;
    private GameObject frac;
    public bool onStartDestroy;
    private void Start()
    {
        frac = Main.Resource.Instantiate($"{Prefab.name}.prefab", transform.parent, false, true);
        frac.SetActive(false);
        if (onStartDestroy)
            Invoke("Destroying", 2f);
    }
    public void Destroying()
    {
        frac.transform.position = transform.GetChild(0).position;
        frac.transform.rotation = transform.GetChild(0).rotation;
        frac.transform.localScale = transform.localScale;
        frac.GetComponent<SoundEffect>().PlaySFX();
        Vector3 position = transform.position;

        frac.SetActive(true);
        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
        

        Main.Resource.Destroy(gameObject);
    }
    public void Restoring()
    {
        frac.transform.position = transform.position;
        frac.transform.rotation = transform.rotation;
        frac.transform.localScale = transform.localScale;
        frac.SetActive(true);

        Main.Resource.Destroy(gameObject);
    }
}
