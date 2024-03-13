using UnityEngine;

public class ContinuousAccelerationZone : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float acceleration = 10f, speed = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<DestroyedObject>() == null) return;
        if (other.GetComponent<DestroyedObject>().Type == ObjectPropertyType.RESTORE) return;
        Rigidbody body = other.attachedRigidbody;
        if (body)
        {
            Accelerate(body);
        }
    }

    private void Accelerate(Rigidbody body)
    {
        Vector3 velocity = body.velocity;

        if (acceleration > 0f)
        {
            // 목표 속도에 도달할 때까지 부드럽게 속도 변경
            velocity.y = Mathf.MoveTowards(
                velocity.y, speed, acceleration * Time.deltaTime
            );
        }
        else
        {
            // 즉시 목표 속도로 속도 변경
            velocity.y = speed;
        }

        body.velocity = velocity;
    }
}
