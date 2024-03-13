using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class CheckPlate : MonoBehaviour
{
    bool _isGround;
    [SerializeField] private LayerMask parentLayer;
    public LayerMask InteractableLayerMask;
    private GameObject interactCube;


    private void Update()
    {
        CheckPlayerStand();
        CheckInteractive();
    }
    private void CheckPlayerStand() //바닥 체크
    {
        _isGround = Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, out RaycastHit hit,
            Quaternion.identity, 2f, parentLayer, QueryTriggerInteraction.Ignore);

        if (_isGround)
        {
            transform.SetParent(hit.transform);
        }
        else
        {
            transform.SetParent(null);
        }
    }

    private void CheckInteractive()
    {
        bool isInteractive;
        isInteractive = Physics.BoxCast(transform.position, transform.lossyScale / 3.0f, Vector3.down, out RaycastHit hit,
    Quaternion.identity, 2f, InteractableLayerMask, QueryTriggerInteraction.Ignore);
        //TODO: 해리포터와 마법빗자루
        GameObject cube;

        if (isInteractive) //interactive object의 layermask = 7;
        {
            cube = hit.collider.gameObject;
            //print(cube.name);
            //이전 큐브가 있고
            if (interactCube != null)
            {
                //이전 큐브가 현재 밟고 있는 큐브가 아니면
                if (interactCube != cube)
                {
                    if (interactCube.GetComponent<DragRigidbody>() != null)
                    {
                        //이전 큐브 인터랙트 활성화
                        interactCube.GetComponent<DragRigidbody>().IsInteractable = true;
                    }
                    interactCube = cube;
                    //현재 큐브 인터랙트 비활성화
                    if (cube.GetComponent<DragRigidbody>() != null)
                    {
                        cube.GetComponent<DragRigidbody>().IsInteractable = false;
                    }
                }
                //이전 큐브가 현재 밟고 있는 큐브면 이미 비활성화 되었으니 아무것도 안하기
            }
            else
            {
                // 이전 큐브가 없으면 저장 후 인터랙트 비활성화
                interactCube = cube;
                if (cube.GetComponent<DragRigidbody>() != null)
                    cube.GetComponent<DragRigidbody>().IsInteractable = false;
            }
        }
        else
        {
            //현재는 큐브를 밟고 있지 않고 이전에 큐브가 남아 있다면 활성화
            if (interactCube != null && interactCube.GetComponent<DragRigidbody>() != null)
            {
                interactCube.GetComponent<DragRigidbody>().IsInteractable = true;

                interactCube = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawCheckVolume();
    }

    private void DrawCheckVolume() //gizmo그리기
    {
        _isGround = Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, out RaycastHit hit,
            Quaternion.identity, 2f, parentLayer, QueryTriggerInteraction.Ignore);
        if (_isGround)
        {
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale);
        }
        else
        {
            Gizmos.DrawRay(transform.position, Vector3.down * 2f);
        }
    }

}
