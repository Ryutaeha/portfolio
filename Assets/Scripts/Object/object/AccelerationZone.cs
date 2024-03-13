using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    public float accelerationStrength = 10f; // 가속 강도
    private PlayerEventController playerController;
    private bool _isPlayerAccel;

    private void Update()
    {
        if (playerController != null)
        {
            if (!_isPlayerAccel && playerController.accelerate > 0)
            {
                playerController.accelerate -= Time.unscaledDeltaTime * 2f; //가속 패달 밖에서 점차 감속
                if (playerController.IsAccelerator())
                {
                    playerController.accelerate = 0;
                }
                if (playerController.accelerate < 0)
                {
                    playerController.accelerate = 0;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;

        if (body != null)
        {
            ApplyAcceleration(body);
        }

        if (other.CompareTag("Player"))
        {
            _isPlayerAccel = true;
            playerController = other.GetComponent<PlayerEventController>();
            playerController.accelerate = accelerationStrength;
        }
    }

    private void ApplyAcceleration(Rigidbody body)
    {
        Vector3 movementDirection = body.velocity.normalized;
        Vector3 force = movementDirection * accelerationStrength;
        body.AddForce(force, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerAccel= false;
            playerController = other.GetComponent<PlayerEventController>();
            //playerController.accelerate = 0f;
        }
    }
}
