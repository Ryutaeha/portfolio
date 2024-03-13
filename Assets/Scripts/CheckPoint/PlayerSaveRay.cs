using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveRay : MonoBehaviour
{
    [SerializeField] float rayDistance = 10.0f;  // 레이의 거리를 설정합니다.
    [SerializeField] float rayPosition = 10.0f;  // 레이의 범위를 설정합니다.
    [SerializeField] LayerMask player;
    [SerializeField] Transform checkPoint;
    void Update()
    {
        SavePoint();
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Gizmos.color = Color.red; // 레이의 색상을 빨간색으로 설정합니다.

        for (int i = 0; i < 3; i++)
        {
            Vector3 rayOrigin = origin + transform.forward * i;//new Vector3(0, -i * rayPosition, 0);

            // 오른쪽 방향의 레이를 그립니다.
            Gizmos.DrawRay(rayOrigin, transform.right * rayDistance);

            // 왼쪽 방향의 레이를 그립니다.
            Gizmos.DrawRay(rayOrigin, -transform.right * rayDistance);
        }
    }
    private void SavePoint()
    {

        Vector3 origin = transform.position;
        RaycastHit hit;

        for (int i = 0; i < 3; i++)
        {
            Vector3 rayOrigin = origin + transform.forward * i;

            // 오른쪽 방향으로 레이를 발사합니다.
            if (Physics.Raycast(rayOrigin, transform.right, out hit, rayDistance, player))
            {
                ProcessRaycastHit(hit);
            }

            // 왼쪽 방향으로 레이를 발사합니다.
            if (Physics.Raycast(rayOrigin, -transform.right, out hit, rayDistance, player))
            {
                ProcessRaycastHit(hit);
            }
        }
    }
    private void ProcessRaycastHit(RaycastHit hit)
    {
        PlayerCheckPoint checkPoint = hit.transform.GetComponent<PlayerCheckPoint>();
        if (checkPoint != null && checkPoint.LastCheckPoint != this.checkPoint.position)
        {
            checkPoint.CheckPoint(this.checkPoint.position);
            return;
        }
    }
}
