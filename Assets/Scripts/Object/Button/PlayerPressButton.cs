using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPressButton : MonoBehaviour
{
    private Coroutine coroutine;
    private bool isContact;
    private Vector3 downVector;
    public GameObject Door;
    public GameObject NarrationBox;
    private DoorOpen doorOpenScript;
    private DoubleSidedDoorController sidedDoor;

    private void Awake()
    {
        if(!Door.TryGetComponent<DoorOpen>(out doorOpenScript))
            Door.TryGetComponent<DoubleSidedDoorController>(out sidedDoor);
        downVector = new Vector3(0, -0.15f, 0);
    }

    public void PressButtonFromPlayer()
    {
        Transform cubeTransform = this.transform;

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(cubeTransform.DOLocalMove(downVector, 1f));
        mySequence.Play();
        if(NarrationBox != null)
        {
            NarrationBox.SetActive(false);
        }
        doorOpenScript.OpenDoor();
        isContact = true;
    }

    public void PressButtonEndFromPlayer()
    {
        isContact = false;

        coroutine ??= StartCoroutine(Delay());
    }


    private IEnumerator Delay() //버튼이 다시 올라오는 코루틴
    {
        yield return new WaitForSeconds(0.3f);
        if (isContact)
        {
            coroutine = null;
            yield break;
        }
        Transform cubeTransform = this.transform;

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(cubeTransform.DOLocalMove(new Vector3(0, 0.2f, 0), 1f));
        mySequence.Play();
        doorOpenScript.CloseDoor();

        coroutine = null;
    }
}