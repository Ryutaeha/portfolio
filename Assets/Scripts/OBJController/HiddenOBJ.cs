using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class HiddenOBJ : MonoBehaviour
{
    private PlayerCameraMove playerCameraMove;
    [SerializeField] private CameraMoveScriptableObject type;

    // Start is called before the first frame update
    void Start()
    {
        playerCameraMove = new PlayerCameraMove();
        playerCameraMove.SetMoveVersion(type);
        Main.Game.HiddenOBJ += CreateOBJ;
    }

    private void CreateOBJ()
    {
        playerCameraMove.CameraMove(gameObject);
        //gameObject.layer = Main.Game.OnCamera ? OnCameraLayer : OffCameraLayer;
    }
}
