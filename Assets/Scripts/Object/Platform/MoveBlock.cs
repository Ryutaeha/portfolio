using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public Ease ease;

    public Vector3 waypoint1;
    [SerializeField] private float delayTime;
    private Vector3[] wayPoints;
    [HideInInspector] public bool IsMove;

    private Tweener tweener; // Tweener 변수 선언

    private void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        // wayPoints 배열 초기화
        wayPoints = new Vector3[3];

        wayPoints[0] = transform.position; // 첫 번째 웨이포인트는 현재 위치
        wayPoints[1] = waypoint1; // 두 번째 웨이포인트는 waypoint1 값
        wayPoints[2] = transform.position; // 두 번째 웨이포인트는 waypoint1 값

    }

    public void MoveLoop() //시작 시 정해진 경로 이동
    {        
        if (IsMove)
        {
            if(tweener == null)
            {
                // DOTween을 사용하여 물체를 경로로 이동
                tweener = transform.DOPath(wayPoints, delayTime, PathType.Linear)
                    .SetEase(ease)
                    .SetLoops(-1, LoopType.Restart)
                    .SetUpdate(UpdateType.Fixed, false);
            }
            else
            {
                tweener.Play();
            }
        }
        else
        {
            if (tweener != null)
            {
                tweener.Pause(); // Tween 애니메이션을 멈춥니다.
            }
        }
    }

}