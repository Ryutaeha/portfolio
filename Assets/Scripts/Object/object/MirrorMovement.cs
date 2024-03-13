using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    [SerializeField] private Transform leader; // 리더 객체
    [SerializeField] private Vector3 mirrorAxisPosition; // 거울 축의 참조 위치
    public enum MirrorPlane { XY, XZ, YZ } // 거울면 선택
    [SerializeField] private MirrorPlane mirrorPlane = MirrorPlane.XY; // 기본값 설정
    private Outline outline;
    private Outline leaderOutline;
    private bool getOutline;
    private bool leaderGetOutline;

    private void Awake()
    {
        getOutline = TryGetComponent<Outline>(out outline);
        leaderGetOutline = leader.TryGetComponent<Outline>(out leaderOutline);
    }

    void Update()
    {
        if (getOutline && leaderGetOutline)
        {
            if (leaderOutline.enabled == true)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
        // 리더의 위치를 기반으로 팔로워의 위치 계산
        Vector3 mirroredPosition = transform.position;
        Quaternion mirroredRotation = leader.rotation; // 리더의 회전을 기반으로 한 팔로워의 회전

        // 선택된 면에 따라 모든 축의 위치를 반영
        switch (mirrorPlane)
        {
            case MirrorPlane.XY:
                mirroredPosition.x = mirrorAxisPosition.x * 2 - leader.position.x;
                mirroredPosition.y = mirrorAxisPosition.y * 2 + leader.position.y;
                mirroredPosition.z = leader.position.z; // Z축만 반전
                mirroredRotation = Quaternion.Euler(leader.eulerAngles.x, -leader.eulerAngles.y, -leader.eulerAngles.z); // XY면에 대한 회전 반영
                break;
            case MirrorPlane.XZ:
                mirroredPosition.x = leader.position.x;
                mirroredPosition.z = leader.position.z;
                mirroredPosition.y = -leader.position.y; // Y축만 반전
                mirroredRotation = Quaternion.Euler(-leader.eulerAngles.x, leader.eulerAngles.y, -leader.eulerAngles.z); // XZ면에 대한 회전 반영
                break;
            case MirrorPlane.YZ:
                mirroredPosition.y = mirrorAxisPosition.y * 2 + leader.position.y;
                mirroredPosition.z = mirrorAxisPosition.z * 2 - leader.position.z;
                mirroredPosition.x = leader.position.x; // X축만 반전
                mirroredRotation = Quaternion.Euler(-leader.eulerAngles.x, -leader.eulerAngles.y, leader.eulerAngles.z); // YZ면에 대한 회전 반영
                break;
        }

        // 팔로워의 위치 및 회전 업데이트
        transform.position = mirroredPosition;
        transform.rotation = mirroredRotation;
    }
}
