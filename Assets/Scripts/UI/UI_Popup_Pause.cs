using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Pause : UI_Popup
{
    #region enums
    private enum Buttons
    {
        Btn_Continue,
        Btn_Settings,
        Btn_Exit,
    }
    #endregion
    protected override bool Init()
    {
        if (!base.Init()) return false;
        SetUI<Button>();
        SetBtnEvent();
        Cursor.visible = true;

        return true;
    }
    protected override void SetBtnEvent()
    {
        GetUI<Button>(Buttons.Btn_Continue.ToString()).onClick.AddListener(() =>
        {
            //이어하기
            Main.UI.ClosePopupUI();
            Main.Game.ContinueGame();
            Cursor.visible = false;
        });
        GetUI<Button>(Buttons.Btn_Settings.ToString()).onClick.AddListener(() =>
        {
            //세팅
            Main.UI.ShowPopupUI<UI_Popup_Settings>();
        });
        GetUI<Button>(Buttons.Btn_Exit.ToString()).onClick.AddListener(() =>
        {
            Main.Data.PlayerPlaySceneSave();
            Main.Game.ContinueGame();
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.0083f;
            Main.Scenes.LoadScene(Define.Scene.TitleScene);
        });
    }
}
