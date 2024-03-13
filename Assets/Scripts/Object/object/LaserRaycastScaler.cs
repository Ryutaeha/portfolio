using UnityEngine;
using System.Collections;

public class LaserRaycastScaler : MonoBehaviour
{
    [SerializeField] private float scaleDecrement = 0.05f; // 스케일 감소량
    [SerializeField] private float checkInterval = 0.1f; // 레이캐스트 체크 간격
    [SerializeField] private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f); // 최소 스케일 값
    [SerializeField] private float maxDistance = 100f; // 레이캐스트 최대 거리
    [SerializeField] private LayerMask targetLayer; // 대상 오브젝트 레이어

    private Transform targetHitLastFrame; // 마지막 프레임에서 충돌한 대상

    private void Start()
    {
        StartCoroutine(CheckRaycast());
    }

    private IEnumerator CheckRaycast()
    {
        while (true)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, targetLayer);

            if (hasHit)
            {
                // 레이캐스트가 오브젝트와 충돌했을 때
                targetHitLastFrame = hit.transform;
                ScaleObject(targetHitLastFrame, -scaleDecrement); // 스케일 감소
            }
            else if (targetHitLastFrame != null)
            {
                // 충돌이 끝났을 때 (더 이상 충돌하지 않는 경우)
                targetHitLastFrame = null; // 마지막으로 충돌한 대상을 리셋
            }

            yield return new WaitForSeconds(checkInterval); // 설정된 간격으로 레이캐스트 체크
        }
    }

    private void ScaleObject(Transform target, float scaleChange)
    {
        Vector3 newScale = target.localScale + new Vector3(scaleChange, scaleChange, scaleChange);
        newScale = Vector3.Max(minScale, newScale); // 최소 스케일 값 이하로 감소하지 않도록 보장
        target.localScale = newScale;
    }
}
