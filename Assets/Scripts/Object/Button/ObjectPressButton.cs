using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPressButton : MonoBehaviour
{
    private Coroutine coroutine;
    private bool isContact;
    private Rigidbody rb;
    private Vector3 downVector;
    public GameObject Door;
    private DoorOpen doorOpen;
    public GameObject NarrationBox;

    private void Awake()
    {
        doorOpen = Door.GetComponent<DoorOpen>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object")) 
        {
            //Main.Sound.PlayClipInSFX("DoorOpen", Door.transform.position);
        }

    }
    private void OnCollisionStay(Collision collision) //버튼과 오브젝트가 접촉을 유지하고 있는 중일 때
    {
        if (collision.gameObject.CompareTag("Object")) 
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();

            if(rb.mass < 2) //무게에 따라 내려가는 값
            {
                downVector = new Vector3(0, 0.1f, 0);
            }
            else
            {
                downVector = new Vector3(0, -0.15f, 0);
            }

            Transform cubeTransform = this.transform;
                        
            DG.Tweening.Sequence mySequence = DOTween.Sequence(); // 시퀀스를 생성합니다.

            mySequence.Append(cubeTransform.DOLocalMove(downVector, 1f)); //원하는 위치까지 1초의 duration

            mySequence.Play(); //시퀀스 재생

            doorOpen.OpenDoor();
            if (NarrationBox != null)
            {
                NarrationBox.SetActive(false);
            }

            isContact = true;
        }
    }
    private void OnCollisionExit(Collision collision) //접촉이 끝났을 때
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isContact = false;

            if (coroutine == null)
            {
                coroutine = StartCoroutine(Delay()); 
            }
        }
    }

    private IEnumerator Delay() //버튼이 다시 올라오는 코루틴
    {
        yield return new WaitForSeconds(0.5f);
        if (isContact)
        {
            coroutine = null;
            yield break;
        }
        Transform cubeTransform = this.transform;

        DG.Tweening.Sequence mySequence = DOTween.Sequence();

        mySequence.Append(cubeTransform.DOLocalMove(new Vector3(0, 0.2f, 0), 1f));

        mySequence.Play();

        doorOpen.CloseDoor();

        coroutine = null;
    } 

}
