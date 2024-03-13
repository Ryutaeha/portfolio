
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Interact : MonoBehaviour
{
    protected float maxCheckDistance;
    protected LayerMask layerMask;
    protected LayerMask DummylayerMask;

    public Camera _camera;

    [HideInInspector] public GameObject curInteractObject;
    protected Outline curInteractOutline;
    protected bool curInteractObjectCheck = false;
    protected UI_GameScene _gameSceneUI;

    protected virtual void Update()
    {
        TargetingObject();
        TargetingObjectCheck();
        //Debug.Log(maxCheckDistance);
    }
    private void TargetingObject()
    {

        if (curInteractObject != null && curInteractObjectCheck) return;

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);
        if (Physics.Raycast(ray, out hit, maxCheckDistance, DummylayerMask))
        {
            if ((layerMask & (1 << hit.transform.gameObject.layer)) == 0)
            {
                if (curInteractOutline != null && !curInteractObjectCheck)
                {
                    if (curInteractObject.transform.parent != null && curInteractObject.transform.parent.CompareTag("DestroyObject"))
                    {
                        Debug.Log(curInteractObject.transform.parent.name);
                        foreach (var outlines in curInteractObject.transform.parent.GetComponentsInChildren<Outline>())
                        {
                            outlines.enabled = false;
                        }
                    }
                    curInteractOutline.enabled = false;
                    curInteractOutline = null;
                    curInteractObject = null;

                }
            }
            else if (hit.collider != null)
            {

                if (curInteractOutline != null && hit.collider.gameObject == curInteractOutline.gameObject)
                {
                    return;
                }
                if (curInteractOutline != null && curInteractOutline.gameObject != hit.collider.gameObject)
                {
                    curInteractOutline.enabled = false;
                }
                curInteractObject = hit.collider.gameObject;
                if (hit.collider.TryGetComponent<Outline>(out curInteractOutline))
                {
                    curInteractOutline.enabled = true;
                }
               
                if (curInteractObject.transform.parent != null && curInteractObject.transform.parent.CompareTag("DestroyObject"))
                {
                    foreach (var outlines in hit.collider.transform.parent.GetComponentsInChildren<Outline>())
                    {
                        outlines.enabled = true;
                    }
                }

            }
        }
        else
        {
            if (curInteractOutline != null && !curInteractObjectCheck)
            {
                if (curInteractObject.transform.parent != null && curInteractObject.transform.parent.CompareTag("DestroyObject"))
                {
                    Debug.Log(curInteractObject.transform.parent.name);
                    foreach (var outlines in curInteractObject.transform.parent.GetComponentsInChildren<Outline>())
                    {
                        outlines.enabled = false;
                    }
                }
                if (curInteractOutline != null)
                {
                    curInteractOutline.enabled = false;
                }
                curInteractOutline = null;
                curInteractObject = null;

            }
        }
    }
    void TargetingObjectCheck()
    {
        if (curInteractObject != null)
        {
            Vector3 viewPos = _camera.WorldToViewportPoint(curInteractObject.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                return;
            }
            else
            {
                // curInteractObject.GetComponent<ReplayRecorder>().isClick = false;
                if (curInteractObject.GetComponent<ReplayRecorder>() == null)
                {
                    if (curInteractObject.GetComponent<DestroyedObject>() != null)
                        curInteractObject.GetComponent<DestroyedObject>().Recording(false, false);
                }
                else
                {
                    curInteractObject.GetComponent<ReplayRecorder>().Recording(false, false);
                }
                if (curInteractObject.GetComponent<FloatingPath>() != null)
                {
                    DOTween.PlayForward(curInteractObject.transform);
                }

                if (curInteractObject.transform.parent != null && curInteractObject.transform.parent.CompareTag("DestroyObject"))
                {
                    foreach (var outlines in curInteractObject.transform.parent.GetComponentsInChildren<Outline>())
                    {
                        outlines.enabled = false;
                    }
                }

                if (curInteractOutline != null)
                {
                    curInteractOutline.enabled = false;
                }
                curInteractObject = null;
                curInteractOutline = null;

                if (_gameSceneUI == null)
                    _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;
            }
        }
    }
}



