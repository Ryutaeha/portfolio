using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UnifiedPressButton : MonoBehaviour
{
    public enum PressButtonType
    {
        PlayerOnly,
        ObjectOnly,
        Both
    }
    public enum DoorType
    {
        Sliding, // 슬라이딩 문
        Swing // 스윙(여닫이) 문
    }

    public PressButtonType pressButtonType;
    private Coroutine coroutine;
    private bool isContact;
    public GameObject Door;
    public GameObject NarrationBox;
    private DoorOpen doorOpenScript;
    private DoubleSidedDoorController doubleSidedDoorController;
    private Vector3 downVector;
    private Vector3 upVector;

    private void Awake()
    {
        Door.TryGetComponent<DoorOpen>(out doorOpenScript);
        Door.TryGetComponent<DoubleSidedDoorController>(out doubleSidedDoorController);
        downVector = new Vector3(0, -0.15f, 0);
        upVector = new Vector3(0, 0.2f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ShouldReact(collision.gameObject))
        {
            // 여기에 필요한 동작을 추가합니다. 예: 소리 재생
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (ShouldReact(collision.gameObject))
        {
            ProcessPress(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (ShouldReact(collision.gameObject))
        {
            isContact = false;
            coroutine ??= StartCoroutine(ButtonReleaseDelay());
        }
    }

    public void PressButtonFromPlayer()
    {
        if (pressButtonType != PressButtonType.ObjectOnly)
        {
            ProcessPress(null);
        }
    }

    public void PressButtonEndFromPlayer()
    {
        if (pressButtonType != PressButtonType.ObjectOnly)
        {
            isContact = false;
            coroutine ??= StartCoroutine(ButtonReleaseDelay());
        }
    }

    private void ProcessPress(GameObject colliderObject)
    {
        Rigidbody rb = colliderObject?.GetComponent<Rigidbody>();

        if (rb != null && rb.mass < 2 && pressButtonType != PressButtonType.PlayerOnly)
        {
            downVector = new Vector3(0, 0.1f, 0);
        }
        else
        {
            downVector = new Vector3(0, -0.15f, 0);
        }

        Transform buttonTransform = transform;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(buttonTransform.DOLocalMove(downVector, 1f)).Play();

        doorOpenScript?.OpenDoor();
        doubleSidedDoorController?.OpenDoor();
        if (NarrationBox != null)
        {
            NarrationBox.SetActive(false);
        }

        isContact = true;
    }

    private IEnumerator ButtonReleaseDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (isContact)
        {
            coroutine = null;
            yield break;
        }

        Transform buttonTransform = transform;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(buttonTransform.DOLocalMove(upVector, 1f)).Play();

        doorOpenScript?.CloseDoor();
        doubleSidedDoorController?.CloseDoor();
        coroutine = null;
    }

    private bool ShouldReact(GameObject collider)
    {
        switch (pressButtonType)
        {
            case PressButtonType.PlayerOnly:
                return collider.CompareTag("Player");
            case PressButtonType.ObjectOnly:
                return collider.CompareTag("Object");
            case PressButtonType.Both:
                return collider.CompareTag("Player") || collider.CompareTag("Object");
            default:
                return false;
        }
    }
}
