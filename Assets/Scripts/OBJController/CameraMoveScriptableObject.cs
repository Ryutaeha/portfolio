using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICameraMove
{
    void CameraMove(GameObject gameObject);
}
public abstract class CameraMoveScriptableObject : ScriptableObject, ICameraMove
{
    public abstract void CameraMove(GameObject gameObject);
}