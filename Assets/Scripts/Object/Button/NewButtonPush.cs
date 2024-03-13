using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewButtonPush : MonoBehaviour
{
    public GameObject MovePillar;
    public string StatueName;

    public Vector3 Before;
    public Vector3 After;

    private bool IsLoop;
    private bool isContact;

    public Material LightMaterial;
    public Material BeforeMaterial;

    private Coroutine coroutine;
    private Renderer _renderer;
    private MoveBlock moveBlock;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = BeforeMaterial;

        if (MovePillar.TryGetComponent<MoveBlock>(out moveBlock))
        {
            moveBlock = MovePillar.GetComponent<MoveBlock>();
            IsLoop = true;
        }
        else
        {
            IsLoop = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsLoop)
        {
            moveBlock.IsMove = true;
            moveBlock.MoveLoop();
        }
    }

    #region 발판에 올라온 경우
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == StatueName)
        {
            Debug.Log("접촉중");
            _renderer.material = LightMaterial;
            if (!IsLoop)
            {
                Transform pillarTransform = MovePillar.transform;

                //Sequence mySequence = DOTween.Sequence(); // 시퀀스를 생성합니다.

                //mySequence.Append();
                pillarTransform.DOMove(After, 3f).SetUpdate(UpdateType.Fixed, false);
                //mySequence.Play(); //시퀀스 재생   

                isContact = true;
            }

        }
    }
    #endregion

    #region 발판에서 내려온 경우
    private void OnCollisionExit(Collision collision)
    {
        _renderer.material = BeforeMaterial;

        if (collision.gameObject.name == StatueName)
        {
            if (!IsLoop)
            {
                Debug.Log("접촉종료");
                isContact = false;

                if (coroutine == null)
                {
                    coroutine = StartCoroutine(Delay());
                }
            }
            else
            {
                moveBlock.IsMove = false;
                moveBlock.MoveLoop();
            }
        }
    }

    private IEnumerator Delay() //기둥이 다시 올라오는 코루틴
    {
        yield return new WaitForSeconds(0.3f);
        if (isContact)
        {
            coroutine = null;
            yield break;
        }

        Transform pillarTransform = MovePillar.transform;

        pillarTransform.DOMove(Before, 3f).SetUpdate(UpdateType.Fixed, false);
        //Sequence mySequence = DOTween.Sequence(); // 시퀀스를 생성합니다.

        //mySequence.Append();

        //mySequence.Play(); //시퀀스 재생   

        coroutine = null;
    }
    #endregion

}