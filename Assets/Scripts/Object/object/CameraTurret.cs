using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurret : MonoBehaviour
{
    private Transform Target;
    [SerializeField] private float interval = 20f;
    private float detection; //감지영역
    private Quaternion initialRotation;
    private Coroutine rotationCoroutine;
    public GameObject laserPrefab; // 레이저 프리팹
    private GameObject laser; // 활성화된 레이저
    private Vector3 laserStartPosition;

    private void Start()
    {
        detection = interval + 10f;
        Target = Main.Game.Player.transform;
        initialRotation = transform.rotation;
        laserStartPosition = transform.position + new Vector3(0, 2.52f, 0); //카메라 랜즈에서 레이저 시작점
        laser = Instantiate(laserPrefab, laserStartPosition, Quaternion.identity); // 레이저 초기화
        laser.SetActive(false); // 초기에는 레이저를 비활성화
    }

    private void Update()
    {
        Vector3 dir = Target.position - laserStartPosition;
        float difference = Vector3.Distance(Target.position, laserStartPosition);

        if (difference < detection) //플레이어가 감지범위 안에 있는 경우
        {
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
                rotationCoroutine = null;
            }

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y + 90, targetRotation.eulerAngles.x), Time.deltaTime * 5); // 부드러운 회전으로 플레이어를 향함
            laser.transform.rotation = targetRotation; // 레이저가 플레이어를 정확히 향하도록 설정
            if(difference < interval)
            {
                laser.SetActive(true); // 플레이어가 공격 범위 내에 있을 때 레이저 활성화
            }
        }
        else if (rotationCoroutine == null)
        {
            if (laser.activeSelf)
            {
                laser.SetActive(false); // 플레이어가 범위 밖으로 나갔을 때 레이저 비활성화
            }
            rotationCoroutine = StartCoroutine(RotateBackToInitial());
        }
    }

    private IEnumerator RotateBackToInitial()
    {
        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * 10); // 부드러운 회전으로 초기 상태로 복귀
            yield return null;
        }
    }
}
