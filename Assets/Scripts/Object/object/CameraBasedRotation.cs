using UnityEngine;

public class CameraBasedRotation : MonoBehaviour
{
    public Transform playerCamera; // 플레이어 카메라
    private Quaternion targetRotation; // 목표 회전
    private bool isRotating = false; // 회전 중인지 여부
    private float rotationSpeed = 1.0f; // 회전 속도

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 카메라 방향을 기준으로 시계 방향으로 90도 회전
            StartRotation(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 카메라 방향을 기준으로 반시계 방향으로 90도 회전
            StartRotation(false);
        }

        // 부드러운 회전 실행
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) // 회전 완료 조건
            {
                transform.rotation = targetRotation; // 정확한 목표 회전으로 설정
                isRotating = false;
            }
        }
    }

    private void StartRotation(bool clockwise)
    {
        Vector3 rotationAxis = playerCamera.forward; // 카메라의 전방 벡터를 회전 축으로 사용
        if (!clockwise)
        {
            rotationAxis = -rotationAxis;
        }

        targetRotation = Quaternion.AngleAxis(90, rotationAxis) * transform.rotation;
        isRotating = true;
    }
}
