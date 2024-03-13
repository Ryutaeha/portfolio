using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Clear()
    {
        Main.Resource.ReleaseAllAsset(Main.NextScene);
    }
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //Main.Resource.LoadAllAsync<UnityEngine.Object>("TitleScene", (key, count, totalCount) => {
        //    Debug.Log($"[GameScene] Load asset {key} ({count}/{totalCount})");
        //    if (count >= totalCount)
        //    {
        //        SceneType = Define.Scene.TitleScene;
        //        CurSceneUI = Main.UI.ShowSceneUI<UI_TitleScene>();
        //        Cursor.lockState = CursorLockMode.None;
        //    }
        //});
        SceneType = Define.Scene.TitleScene;
        CurSceneUI = Main.UI.ShowSceneUI<UI_TitleScene>();
        Cursor.lockState = CursorLockMode.None;
        return true;
    }
}
