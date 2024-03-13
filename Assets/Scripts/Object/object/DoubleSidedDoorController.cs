using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoubleSidedDoorController : MonoBehaviour
{
    public enum Angle
    {
        IN,
        OUT
    }
    public enum Type
    {
        FIRST,
        ONOFF,
    }
    public Transform leftDoorParent; // 왼쪽 문의 부모 오브젝트 Transform
    public Transform rightDoorParent; // 오른쪽 문의 부모 오브젝트 Transform
    public float openAngle = 90.0f; // 문이 열릴 때의 각도
    public float animationTime = 2.0f; // 문이 완전히 열리거나 닫히는 데 걸리는 시간

    private bool isOpen = false; // 문이 현재 열려 있는지 여부
    [SerializeField] private Type doorType;
    [SerializeField] private Angle doorAngle;
    private void Awake()
    {
        if (doorType == Type.FIRST)
        {
            TriggerDoor();
        }
    }

    // 문을 열거나 닫는 코루틴
    IEnumerator ToggleDoor(bool open)
    {
        float time = 0;
        Quaternion leftStartRotation = leftDoorParent.rotation;
        Quaternion rightStartRotation = rightDoorParent.rotation;
        Quaternion leftEndRotation;
        Quaternion rightEndRotation;

        if (doorAngle == Angle.IN)
        {
            if (open)
            {
                leftEndRotation = Quaternion.Euler(0, openAngle, 0); // 왼쪽 문을 열기 위한 최종 회전 각도
                rightEndRotation = Quaternion.Euler(0, -openAngle, 0); // 오른쪽 문을 열기 위한 최종 회전 각도
            }
            else
            {
                leftEndRotation = Quaternion.Euler(0, 0, 0); // 왼쪽 문을 닫기 위한 최종 회전 각도
                rightEndRotation = Quaternion.Euler(0, 0, 0); // 오른쪽 문을 닫기 위한 최종 회전 각도
            }
        }
        else
        {
            if (open)
            {
                leftEndRotation = Quaternion.Euler(0, -openAngle, 0); // 왼쪽 문을 열기 위한 최종 회전 각도
                rightEndRotation = Quaternion.Euler(0, openAngle, 0); // 오른쪽 문을 열기 위한 최종 회전 각도
            }
            else
            {
                leftEndRotation = Quaternion.Euler(0, 180, 0); // 왼쪽 문을 닫기 위한 최종 회전 각도
                rightEndRotation = Quaternion.Euler(0, 180, 0); // 오른쪽 문을 닫기 위한 최종 회전 각도
            }
        }
        

        while (time < animationTime)
        {
            leftDoorParent.rotation = Quaternion.Slerp(leftStartRotation, leftEndRotation, time / animationTime);
            rightDoorParent.rotation = Quaternion.Slerp(rightStartRotation, rightEndRotation, time / animationTime);
            time += Time.deltaTime;
            yield return null;
        }

        leftDoorParent.rotation = leftEndRotation; // 왼쪽 문 애니메이션 완료 후 최종 각도로 설정
        rightDoorParent.rotation = rightEndRotation; // 오른쪽 문 애니메이션 완료 후 최종 각도로 설정
        isOpen = open; // 문 상태 업데이트
    }
    public void OpenDoor()
    {
        TriggerDoor();
    }
    public void CloseDoor()
    {
        TriggerDoor();
    }
    // 문을 열고 닫는 함수
    private void TriggerDoor()
    {
        StartCoroutine(ToggleDoor(!isOpen));
    }
}
