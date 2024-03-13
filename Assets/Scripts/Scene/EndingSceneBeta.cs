using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneBeta : BaseScene
{
    public override void Clear()
    {
        Main.Resource.ReleaseAllAsset(Main.NextScene);
    }

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        return true;
    }
    public void End()
    {

        Main.Scenes.LoadScene(Define.Scene.TitleScene);
        
    }
}
