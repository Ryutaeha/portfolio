using UnityEngine;

// Drag a rigidbody with the mouse using a spring joint like Dani did in KARLSON. By Boxply
// https://youtube.com/c/Boxply

[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;
    private float distance = 10;

    private LineRenderer lr;
    private Transform lineRenderLocation;
    private Transform jointTrans;
    private float dragDepth;
    public bool IsInteractable;
    private void Start()
    {

        lr = Main.Game.Player.transform.GetComponentInChildren<LineRenderer>();
        lineRenderLocation = lr.transform;
        IsInteractable = true;
    }

    void OnMouseDown()
    {
        if (IsInteractable)
            HandleInputBegin(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if (IsInteractable)
            HandleInputEnd(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        if (IsInteractable)
            HandleInput(Input.mousePosition);
        else
            HandleInputEnd(Input.mousePosition);
    }

    public void HandleInputBegin(Vector3 screenPosition)
    {
        if(jointTrans != null)
        {
            Destroy(jointTrans.gameObject);
        }

        var ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, (1 << 7) | (1 << 14)))
        {
            dragDepth = CameraPlane.CameraToPointDepth(Camera.main, hit.point);
            jointTrans = AttachJoint(hit.rigidbody, hit.point);
        }

        lr.positionCount = 2;
    }

    public void HandleInput(Vector3 screenPosition)
    {
        if (jointTrans == null)
            return;
        if(!IsInteractable)
        {
            HandleInputEnd(screenPosition);
            return;
        }

        var worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        jointTrans.position = CameraPlane.ScreenToWorldPlanePoint(Camera.main, dragDepth, screenPosition);

        DrawRope();
    }

    public void HandleInputEnd(Vector3 screenPosition)
    {
        DestroyRope();
        if(jointTrans!=null)
            Destroy(jointTrans.gameObject);
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        //drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    private void DrawRope()
    {
        if (jointTrans == null)
        {
            return;
        }

        lr.SetPosition(0, lineRenderLocation.position);
        lr.SetPosition(1, this.transform.position);
    }

    private void DestroyRope()
    {
        lr.positionCount = 0;
    }
}
