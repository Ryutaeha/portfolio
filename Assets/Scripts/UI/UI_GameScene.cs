using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    #region enums
    private enum Images
    {
        Ability1,
        Ability2,
        Ability3,
        Ability4,
    }

    #endregion

    #region fields
    [SerializeField] private GameObject _cameraPanel;
    [SerializeField] private GameObject _record;
    [SerializeField] private GameObject _rewind;
    [SerializeField] private GameObject _ability;
    [SerializeField] private GameObject _Tip;
    [SerializeField] private GameObject _keyboardTip;
    [SerializeField] private GameObject _moveTip;
    [SerializeField] private GameObject _timeTip;
    [SerializeField] private GameObject _coolTimeImage;
    [SerializeField] private Image _coolBlack;
    bool isFirstKeyboardTip;
    private GameObject[] _abilities;
    //private CharacterController _characterController;
    #endregion
    private void Update()
    {
        //if (isFirstKeyboardTip&&(Input.GetKey(KeyCode.P)|| Input.GetKey(KeyCode.Escape)))
        //{
        //    _keyboardTip.SetActive(false);
        //    _characterController.enabled = true;
        //}

        if (Input.GetKey(KeyCode.P))
        {
            _Tip.SetActive(true);
        }
        else
        {
            _Tip.SetActive(false);
        }
    }
    protected override bool Init()
    {
        if (!base.Init()) return false;
        SetUI<Image>();

        string[] names = Enum.GetNames(typeof(Images));
        _abilities = new GameObject[names.Length];
        //_characterController = Main.Game.Player.GetComponent<CharacterController>();
        for (int i = 0; i < _abilities.Length; i++)
        {
            _abilities[i] = GetUI<Image>(names[i]).gameObject;
        }

        Cursor.visible = false;
        return true;
    }

    public void SetAbilityUI(int currentMotion)
    {
        for (int i = 0; i < _abilities.Length; i++)
        {
            if (i == currentMotion)
                _abilities[i].SetActive(true);
            else
                _abilities[i].SetActive(false);
        }
    }

    public void ShowKeyboardTip()
    {
        HideMoveTip();
        isFirstKeyboardTip = true;
        _keyboardTip.SetActive(true);
    }
    public void HideKeyboardTip()
    {
        _keyboardTip.SetActive(false);
    }
    public void ShowMoveTip()
    {
        if(!_keyboardTip.activeSelf)
            _moveTip.SetActive(true);
    }
    public void HideMoveTip()
    {
        _moveTip.SetActive(false);
    }
    public void ShowTimeTip()
    {
        HideKeyboardTip();
        HideMoveTip();
        _timeTip.SetActive(true);
    }
    public void HideTimeTIp()
    {
        _timeTip.SetActive(false);
    }
    public void SetCoolTimeImage(float t)
    {
        StopAllCoroutines();
        StartCoroutine(CoCoolTimeImage(t));
    }
    IEnumerator CoCoolTimeImage(float t)
    {
        _coolTimeImage.SetActive(true);
        float cooltime = t;
        while (t>0)
        {
            _coolBlack.fillAmount = t / cooltime;
            t -= Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        _coolTimeImage.SetActive(false);
    }
    private void OnDestroy()
    {
        Main.Data.PlayerPlaySceneSave();
    }
}
