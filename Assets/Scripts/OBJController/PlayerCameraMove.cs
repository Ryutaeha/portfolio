using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCameraMove
{
    private ICameraMove camera;

    public void SetMoveVersion(ICameraMove camera)
    {
        this.camera = camera;
    }

    public void CameraMove(GameObject gameObject)
    {
        camera.CameraMove(gameObject);
    }
}