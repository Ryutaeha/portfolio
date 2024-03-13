using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPath : MonoBehaviour
{
    public Ease ease;

    public Transform waypoint1;
    public Transform waypoint2;
    public Transform waypoint3;

    private Vector3[] wayPoints;

    private void Start() //시작 시 정해진 경로 이동
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        wayPoints = new Vector3[4];
        wayPoints.SetValue(waypoint1.position, 0);
        wayPoints.SetValue(waypoint2.position, 1);
        wayPoints.SetValue(waypoint3.position, 2);
        wayPoints.SetValue(transform.position, 3);
        transform.DOPath(wayPoints, 6.0f, PathType.Linear).SetEase(ease).SetLoops(-1, LoopType.Restart).SetUpdate(UpdateType.Fixed, false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(120);
        }
    }
}
