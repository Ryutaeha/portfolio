using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : Interact
{
    protected CameraController _camController;
    private enum SelectMotion
    {
        Back,
        Front,
        PlayStop
    }
    //[SerializeField] private float ChangeMaxCheckDistance;
    [SerializeField] private LayerMask ChangelayerMask;
    [SerializeField] private LayerMask DumlayerMask;
    private SelectMotion currentMotion = SelectMotion.Back;

    private void Start()
    {
        maxCheckDistance = 10;
        layerMask = ChangelayerMask;
        DummylayerMask = DumlayerMask;
        _camController = GetComponent<CameraController>();


        NowMotion((int)currentMotion);
    }


    protected override void Update()
    {
        if (Main.Game.CurState != GameState.PLAY)
            return;
        base.Update();
    }
    public void OnGravityChange(InputAction.CallbackContext context)
    {
        if (Main.Game.CurState != GameState.PLAY || !_camController.isCameraOn()|| curInteractObject == null)
            return;
        curInteractObject.TryGetComponent<PlayerViewGravity>(out var comp);
        if (comp == null) return;
        comp.SetGravityDir(-_camController.GetCamTransform().up);
    }
    public void OnLeftClickCanceled(InputAction.CallbackContext context)
    {
        if (Main.Game.CurState != GameState.PLAY)
            return;
        if (context.phase == InputActionPhase.Canceled)
        {
            CancelRewind();
        }
    }
    public void CancelRewind()
    {
        curInteractObjectCheck = false;
        if (currentMotion == SelectMotion.PlayStop && curInteractObject != null)
        {
            //curInteractObject.GetComponent<Test>().PlayStop();
        }
        else if (currentMotion == SelectMotion.Back && curInteractObject != null)
        {
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
        }
        else if (currentMotion == SelectMotion.Front && curInteractObject != null)
        {
        }
        if (_gameSceneUI == null)
            _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;
    }

    public void OnLeftClickPlayStop(InputAction.CallbackContext context)
    {//Right인데 오타
        if (Main.Game.CurState != GameState.PLAY || !_camController.isCameraOn())
            return;
        if (context.phase == InputActionPhase.Performed)
        {
            print(Time.time);
            curInteractObjectCheck = true;
            if (currentMotion == SelectMotion.PlayStop && curInteractObject != null)
            {
                //curInteractObject.GetComponent<Test>().PlayStop();
            }
            else if (currentMotion == SelectMotion.Back && curInteractObject != null)
            {
                if (curInteractObject.GetComponent<ReplayRecorder>() == null)
                {
                    if (curInteractObject.GetComponent<DestroyedObject>() != null)
                        curInteractObject.GetComponent<DestroyedObject>().Recording(true, true);
                }
                else
                {
                    curInteractObject.GetComponent<ReplayRecorder>().Recording(true, true);
                }

                if (curInteractObject.GetComponent<FloatingPath>() != null) //움직이는 발판인 경우 애니메이션 되감기
                {
                    DOTween.PlayBackwards(curInteractObject.transform);
                }
            }
            if (_gameSceneUI == null)
                _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;
        }
    }
    /// <summary>
    /// 현재 사용하지 않음 하지만 어디에 연결되어있을지 모르니 일단 삭제하는 하지 않고 있음
    /// </summary>
    /// <param name="context"></param>
    public void OnScrollPerformed(InputAction.CallbackContext context)
    {
        if (Main.Game.CurState != GameState.PLAY || !_camController.isCameraOn())
            return;
        Vector2 scrollDelta = context.ReadValue<Vector2>();

        if (context.phase == InputActionPhase.Started && !curInteractObjectCheck)
        {
            if (scrollDelta.y < 0)
            {
                // 마우스 휠이 아래로 움직였을 때 다음 열거형 값으로 이동
                int nextMotion = ((int)currentMotion + 1) % 3;
                currentMotion = (SelectMotion)nextMotion;
                NowMotion((int)currentMotion);
                //Debug.Log($"Mouse wheel scrolled up. Current motion: {currentMotion}");
            }
            else if (scrollDelta.y > 0)
            {
                // 마우스 휠이 위로 움직였을 때 이전 열거형 값으로 이동
                int nextMotion = ((int)currentMotion - 1 + 3) % 3;
                currentMotion = (SelectMotion)nextMotion;
                NowMotion((int)currentMotion);
                //Debug.Log($"Mouse wheel scrolled down. Current motion: {currentMotion}");
            }
        }
    }
    private void NowMotion(int currentMotion)
    {
        if (_gameSceneUI == null)
            _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;

        _gameSceneUI?.SetAbilityUI(currentMotion);
    }

    public void SetInteractDistance(float value)
    {
        maxCheckDistance = value;
    }
}
