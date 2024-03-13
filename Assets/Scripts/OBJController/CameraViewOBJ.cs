using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraViewOBJ", menuName = "Camera Moves/CameraViewOBJ")]
public class CameraViewOBJ : CameraMoveScriptableObject
{
    
    public override void CameraMove(GameObject gameObject)
    {
        gameObject.layer = Main.Game.OnCamera ? 7 : 15;
    }

}
