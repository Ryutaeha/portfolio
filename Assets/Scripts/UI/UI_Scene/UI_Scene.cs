using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UI_Scene : UIBase
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        Main.UI.SetCanvas(gameObject, false);

        return true;
    }
}