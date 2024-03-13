using UnityEngine;

public class PlayerViewGravity : MonoBehaviour
{
    private Rigidbody rb;
    private Transform playerCamera;
    private float gravityStrength = 9.81f;
    private Vector3 gravityDirection;
    private bool shouldApplyGravity;
    private int count = 0;
    private bool check;
    public bool OnAwake;
    //Renderer rend;
    private void Start()
    {
        playerCamera = Main.Game.Player.GetComponentsInChildren<Camera>()[2].transform;
        rb = GetComponent<Rigidbody>();
        check = Main.Game.OnCamera;
        rb.useGravity = false;
        if(OnAwake)
            gravityDirection = Vector3.down;
        shouldApplyGravity = true;
        //rend = GetComponent<Renderer>();
    }

    //void Update()
    //{
    //    if (playerCamera.rotation.eulerAngles.z == 180 && Input.GetKeyDown(KeyCode.R))
    //    {
    //        gravityDirection = -playerCamera.up;

    //    }
    //    else if (playerCamera.rotation.eulerAngles.z == 0 && Input.GetKeyDown(KeyCode.R))
    //    {
    //        gravityDirection = -playerCamera.up;

    //    }
    //    else if (playerCamera.rotation.eulerAngles.z == 90 && Input.GetKeyDown(KeyCode.R))
    //    {
    //        gravityDirection = -playerCamera.up;

    //    }
    //    else if (playerCamera.rotation.eulerAngles.z == 270 && Input.GetKeyDown(KeyCode.R))
    //    {
    //        gravityDirection = -playerCamera.up;

    //    }

    //}

    void FixedUpdate()
    {
        if (shouldApplyGravity)
        {
            rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
        }
    }

    private void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        shouldApplyGravity = true;
    }
    public void SetGravityDir(Vector3 dir)
    {
        //rend.material.SetVector("_Gravity", dir);
        gravityDirection = dir;
    }
}
