using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraViewControllReset", menuName = "Camera Moves/CameraViewControllReset")]
public class CameraViewControllReset : CameraMoveScriptableObject
{
    private class LocationOBJ
    {
        public Vector3 position;
        public quaternion rotation;
    }

    private Dictionary<GameObject, LocationOBJ> positions = new Dictionary<GameObject, LocationOBJ>();

    public override void CameraMove(GameObject gameObject)
    {
        Action<GameObject> myAction = Main.Game.OnCamera ? SaveTransform : LoadTransform;
        myAction(gameObject);
    }

    private void SaveTransform(GameObject gameObject)
    {
        LocationOBJ obj = new LocationOBJ();
        obj.position = gameObject.transform.position;
        obj.rotation = gameObject.transform.rotation;
        positions[gameObject] = obj;
    }

    private void LoadTransform(GameObject gameObject)
    {
        if (positions.TryGetValue(gameObject, out LocationOBJ savedPosition))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.position = savedPosition.position;
            gameObject.transform.rotation = savedPosition.rotation;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            positions.Remove(gameObject);
        }
    }
}
