using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraViewOBJ", menuName = "Camera Moves/CameraViewMonster")]
public class CameraViewMonster : CameraMoveScriptableObject
{
    
    public override void CameraMove(GameObject gameObject)
    {
        gameObject.layer = Main.Game.OnCamera ? 7 : 0;
    }

}
