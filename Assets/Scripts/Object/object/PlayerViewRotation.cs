using UnityEngine;

public class PlayerViewRotation : MonoBehaviour
{

    private float rotationSpeed = 5f; // 회전 속도
    private float targetRotation = 0f; // 목표 회전 각도

    void Update()
    {
        // 회전 명령 처리
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetRotation -= 90f;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            targetRotation += 90f;
        }

        // 2차원 회전
        if (Mathf.Abs(transform.localEulerAngles.y - targetRotation) > 0.01f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float angleY = Mathf.LerpAngle(transform.localEulerAngles.y, targetRotation, step);
            transform.localEulerAngles = new Vector3(0, angleY, 0);
        }
    }
}

