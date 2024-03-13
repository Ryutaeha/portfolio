using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MainViewOBJ", menuName = "Camera Moves/MainViewOBJ")]
public class MainViewOBJ : CameraMoveScriptableObject
{
    public override void CameraMove(GameObject gameObject)
    {
        gameObject.layer = Main.Game.OnCamera ? 20 : 14;
    }
}
