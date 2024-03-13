using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UI_Popup : UIBase
{
    protected override bool Init()
    {
        if (!base.Init()) return false;

        Main.UI.SetCanvas(gameObject, true);

        return true;
    }
    protected virtual void SetBtnEvent() { }
}
