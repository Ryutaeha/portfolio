using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌");
        if (collision.collider.CompareTag("DestroyObject") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("충격");
            StartCoroutine(CoDissolve());
        }
    }
    private IEnumerator CoDissolve()
    {
        float alpha = 2.0f;
        while (alpha >= 0)
        {
            _renderer.materials[0].SetFloat("_Power", alpha);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            alpha -= Time.unscaledDeltaTime;
        }
        gameObject.SetActive(false);
    }
}
