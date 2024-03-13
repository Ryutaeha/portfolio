using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdTutorialScene : BaseScene
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
        //if (count >= totalCount)
        //{
        SceneType = Define.Scene.GameScene_Tutorial3;
        CurSceneUI = Main.UI.ShowSceneUI<UI_GameScene>();
        Main.Data.PlayerLordSceneSetting();
        //플레이어 스크립트 제작시 변경 필요
        //GameObject Player = Main.Resource.Instantiate("Player.prefab");
        //Player.transform.position = new Vector3(-5.19f, -2.2f, 0.64f);
        //    }
        //});

        return true;
    }
}
