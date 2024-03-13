using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTutorialScene : BaseScene
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
        //        Main.Resource.Instantiate("Tutorial2.prefab");

        //        //플레이어 스크립트 제작시 변경 필요
        //        GameObject Player = Main.Resource.Instantiate("Player.prefab");
        //        Player.transform.position = new Vector3(11.7f, -10.4f, -3.8f);
        //    }
        //});
        SceneType = Define.Scene.GameScene_Tutorial2;
        CurSceneUI = Main.UI.ShowSceneUI<UI_GameScene>();
        //Main.Resource.Instantiate("Tutorial2.prefab");
        Main.Data.PlayerLordSceneSetting();
        //플레이어 스크립트 제작시 변경 필요
        //GameObject Player = Main.Resource.Instantiate("Player.prefab");
        //Player.transform.position = new Vector3(9.2f,-10.6f,-3.9f);
        return true;
    }
}
