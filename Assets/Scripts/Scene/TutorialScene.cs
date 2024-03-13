using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    public override void Clear()
    {
        Main.Resource.ReleaseAllAsset("GameScene");
    }
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //Main.Resource.LoadAllAsync<UnityEngine.Object>("GameScene", (key, count, totalCount) => {
        //    Debug.Log($"[GameScene] Load asset {key} ({count}/{totalCount})");
        //    if (count >= totalCount)
        //    {
        //        SceneType = Define.Scene.TitleScene;
        //        CurSceneUI = Main.UI.ShowSceneUI<UI_GameScene>();
        //    }
        //});
        SceneType = Define.Scene.GameScene_Tutorial;
        CurSceneUI = Main.UI.ShowSceneUI<UI_GameScene>();
        Main.Data.PlayerLordSceneSetting();
        return true;
    }
}
