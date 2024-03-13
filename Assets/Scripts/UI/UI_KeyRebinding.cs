using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_KeyRebinding : MonoBehaviour
{
    private InputActionReference _actionRef = null;
    [SerializeField] private TMP_Text _keyText;
    [SerializeField] private Image _highlights;
    public TMP_Text KeyText { 
        get
        {
            if (_keyText == null)
                _keyText = GetComponentInChildren<TMP_Text>();
            return _keyText;
        } }
    public Image Highlights
    {
        get
        {
            if (_highlights == null)
            {
                Image[] imgs = GetComponentsInChildren<Image>(true);
                foreach (var item in imgs)
                {
                    if(item.gameObject.name!=gameObject.name)
                        _highlights = item;
                }
            }
            return _highlights;
        }
    }
    [SerializeField] private InputBinding.DisplayStringOptions _displayStringOptions;
    private InputActionRebindingExtensions.RebindingOperation _ro;

    //Move Actions의 경우 up/down/left/right 입력해야함
    public string Dir = "";
    public string ActionName = "";
    public int bindingIndex = -1;
    private string path = null;
    public void SetIndex()
    {
        _actionRef = InputActionReference.Create(Main.Data.Actions.FindAction(ActionName));

        //Composite가 아닌 경우 인덱스를 0으로 처리
        //Move Actions처럼 Composite일 경우를 처리하기 위함
        if (Dir.Equals(""))
            bindingIndex = 0;
        else
            bindingIndex = _actionRef.action.bindings.IndexOf(x => x.isPartOfComposite && x.name == Dir);

    }
    public void StartRebinding()
    {
        SetIndex();
        //리바인딩 전 Disable 해야함
        _actionRef.action.Disable();

        //현재 리바인딩할 키의 버튼 하이라이트
        Highlights.gameObject.SetActive(true);

        //Rebind시작하기 전에 변경 전 path 캐싱
        if (_actionRef.action.bindings[bindingIndex].hasOverrides)
            path = _actionRef.action.bindings[bindingIndex].overridePath;
        else
            path = _actionRef.action.bindings[bindingIndex].path;

        //리바인딩
        if (bindingIndex == 0)
        {
            //Composite가 아닐 경우
            _ro = _actionRef.action.PerformInteractiveRebinding()
                .OnCancel(operation => RebindCancel())
                .OnComplete(operation => RebindComplete())
                .Start();
        }
        else
        {
            //Composite일 경우 (Move - Up,Down,Left,Right)
            _ro = _actionRef.action.PerformInteractiveRebinding()
                .WithTargetBinding(bindingIndex)
                .OnCancel(operation => RebindCancel())
                .OnComplete(operation => RebindComplete())
                .Start();
        }
    }

    private void RebindCancel()
    {
        _ro.Dispose();
        _actionRef.action.Enable();
        Highlights.gameObject.SetActive(false);
    }

    private void RebindComplete()
    {
        Highlights.gameObject.SetActive(false);
        _ro.Dispose();
        _actionRef.action.Enable();

        //중복체크 후 중복이면 캐싱해둔 path로 롤백
        if (CheckDuplicateBindings(_actionRef.action))
        {
            if (path != null)
                _actionRef.action.ApplyBindingOverride(bindingIndex,path);
            return;
        }

        ShowBindText();
    }

    public void ShowBindText()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);
        SetIndex();

        displayString = _actionRef.action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, _displayStringOptions);
        KeyText.text = displayString;
    }

    //중복 키 체크
    private bool CheckDuplicateBindings(InputAction action)
    {
        SetIndex();
        int cnt = 0;
        InputBinding newBinding = action.bindings[bindingIndex];
        foreach (InputBinding binding in action.actionMap.bindings)
        {
            //if (binding.action == newBinding.action)
            //    continue;
            if (binding.effectivePath == newBinding.effectivePath)
            {
                cnt++;
            }
        }
        if(cnt > 1)
        {
            Debug.Log("Duplicate binding found : " + newBinding.effectivePath);
            return true;
        }
        else
            return false;
    }
}
