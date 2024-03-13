using UnityEngine;

public class ReflectiveJump : MonoBehaviour
{
    public float jumpStrength = 10f; // 점프 강도
    private PlayerEventController playerController;
    private CharacterController characterController;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody body = collision.rigidbody;
        if (body != null)
        {
            // 첫 번째 접촉 지점에서 입사각과 법선을 얻음
            ContactPoint contact = collision.contacts[0];
            Vector3 incomingVector = body.velocity; // 정규화된 벡터가 아니라 실제 속도 사용
            Vector3 normal = contact.normal;

            // 입사각에 기반한 반사각 계산
            Vector3 reflectVector = Vector3.Reflect(incomingVector, normal).normalized;

            // 반사 방향으로 점프 힘 적용
            body.velocity = reflectVector * jumpStrength;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterController = other.GetComponent<CharacterController>(); 
            Vector3 jumpVector = Vector3.up * jumpStrength;
            characterController.Move(jumpVector);
            Debug.Log("111");
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        characterController = other.GetComponent<CharacterController>();
    //        Vector3 jumpVector = Vector3.up * jumpStrength;
    //        characterController.Move(jumpVector);
    //    }
    //}

}
