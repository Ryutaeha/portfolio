using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_TitleScene : UI_Scene
{
    #region enums
    private enum Buttons
    {
        Btn_Play,
        Btn_Settings,
        Btn_Exit,
        Btn_NewGame,
        Btn_Continue,
    }
    #endregion
    #region fields
    [SerializeField] private GameObject _playTab;
    #endregion
    protected override bool Init()
    {
        if (!base.Init()) return false;
        SetUI<Button>();
        SetBtnEvent();
        return true;
    }
    private void Awake()
    {
        int language;
        if (PlayerPrefs.HasKey("Language"))
        {
            language = PlayerPrefs.GetInt("Language");
        }
        else
        {
            language = 0; //기본 언어 영어
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];        
    }

    private void SetBtnEvent()
    {
        GetUI<Button>(Buttons.Btn_Play.ToString()).onClick.AddListener(() =>
        {
            _playTab.SetActive(true);
        });

        GetUI<Button>(Buttons.Btn_NewGame.ToString()).onClick.AddListener(() =>
        {
            //게임 씬으로

            print("게임시작");
            Main.Scenes.LoadScene(Define.Scene.GameScene_Tutorial);
        });

        GetUI<Button>(Buttons.Btn_Continue.ToString()).onClick.AddListener(() =>
        {
            print("이어하기");
            if(Main.StageClear.isPlaying)
            {
                Debug.Log("저장데이터 있음");
                Main.Data.PlayerPlaySceneLord();
            }
            else
            {
                Debug.Log("저장데이터 없음");
            }
        });
        GetUI<Button>(Buttons.Btn_Settings.ToString()).onClick.AddListener(() =>
        {
            print("환경설정");
            _playTab.SetActive(false);
            Main.UI.ShowPopupUI<UI_Popup_Settings>();
            gameObject.SetActive(false);
        });

        GetUI<Button>(Buttons.Btn_Exit.ToString()).onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        });
    }
}
