using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PunchKing : MonoBehaviour
{
    private bool Punchable;
    float originX;
    float originY;
    float originZ;
    [SerializeField] private Transform _plate;
    private void Awake()
    {
        originX = _plate.position.x;
        originY = _plate.position.y;
        originZ = _plate.position.z;
    }
    private void Update()
    {
        if (Mathf.Approximately(_plate.localPosition.y,-2f)) 
            Punchable = true;
    }
    [SerializeField] private TMP_Text _score;
    private void OnCollisionEnter(Collision collision)
    {
        if (!Punchable)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactive")) //물체를 유리에 던졌을 때
        {
            print("1번 체크포인트");
            if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                print("2번 체크포인트");
                Punchable = false;
                float power = rb.velocity.magnitude;
                _score.text = ((int)power).ToString();
                power = Mathf.Clamp(power, 0, 70);

                Sequence sequence = DOTween.Sequence();
                power = Remap(power, 0, 70, 0, 21);
                print(power);
                sequence.Append(_plate.DOJump(new Vector3(originX, originY + power, originZ), 1f, 1, 1f).SetEase(Ease.Linear));
                sequence.AppendInterval(1f); // 1초 동안 대기
                sequence.Append(_plate.DOMove(new Vector3(originX,originY,originZ), 3f)).SetEase(Ease.Linear);
                sequence.Play();
            }
        }
    }
    public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
    }
}
