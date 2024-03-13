using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _SubcamCanvas;
    [SerializeField] private GameObject subCam;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Vector3 _position1 = new(0.35f, -0.01f, 0.05f);
    [SerializeField] private Vector3 _position2 = new(1, -0.5f, 1f);
    [SerializeField] private float rotateSpeed = 3f;  // 회전 속도
    private IEnumerator rotateCoroutine;
    [SerializeField]private float speed = 5.0f; // 이동 속도
    private bool _cameraChange;
    private bool _enabled = false;
    private bool _enabledRotate = false;
    private InteractController _interactController;
    private PlayerEventController _playerEventController;
    private PlayerInput _playerInput;
    private void Start()
    {
        _cameraChange = Main.Game.OnCamera;
        _interactController = GetComponent<InteractController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerEventController = GetComponent<PlayerEventController>();
    }

    public void CameraMoveCheck()
    {
        if (!_enabled)
        {
            _enabled = true;
            StartCoroutine(CameraMoveCoroutine());
        }
    }

    public void RotationCam(float rot)
    {
        if (_cameraChange && _enabledRotate)
        {
            rotateCoroutine = RotateObjectCoroutine(rot);
            StartCoroutine(rotateCoroutine);
        }
        else
        {
            //로테이션 예외 처리
            _playerInput.enabled = true;
        }
    }
    private IEnumerator RotateObjectCoroutine(float rot)
    {
        _enabledRotate = false;
        Quaternion startRotation = mainCam.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(Vector3.forward * rot);
        float timeElapsed = 0;

        while (timeElapsed < rotateSpeed)
        {
            mainCam.transform.rotation = Quaternion.RotateTowards(mainCam.transform.rotation, endRotation, Time.unscaledDeltaTime * 90 / rotateSpeed);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCam.transform.rotation = endRotation;
        if (rot < 0)
            _playerEventController.SetRotateCount(1);
        else
            _playerEventController.SetRotateCount(-1);
        _enabledRotate = true;
        rotateCoroutine = null;
        _playerInput.enabled = true;
    }
    private IEnumerator CameraMoveCoroutine()
    {
        Vector3 cameraMovePoint = _cameraChange ? _position2 : _position1;

        // 현재 위치와 목표 위치 사이의 거리 계산
        float distance = Vector3.Distance(_camera.transform.localPosition, cameraMovePoint);

        _cameraChange = cameraMovePoint == _position1 ? true : false;
        Main.Game.OnCamera = _cameraChange;

        Debug.Log(_cameraChange);
        Debug.Log(Main.Game.OnCamera);
        if (!_cameraChange)
        {
            if (rotateCoroutine != null)
                StopCoroutine(rotateCoroutine);

            Main.Game.HiddenOBJSet();
            //해당 오브젝트의 로테이션값을 0,0,0으로 변경하고 싶다.
            mainCam.transform.localEulerAngles = Vector3.zero;
            //subCam.enabled = _cameraChange;
            //mainCam.enabled = !_cameraChange;
            _camera.SetActive(!_cameraChange);
            _interactController.CancelRewind();
            _SubcamCanvas.SetActive(false);
            mainCam.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("CameraHiddenOBJ"));
            mainCam.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Monster"));
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Dummy");
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("MainHiddenOBJ");
            //카메라를 내렸음
            _interactController.SetInteractDistance(10);
        }
        // 거리가 충분히 작으면 도착한 것으로 간주
        while (distance >= 0.001f)
        {
            // 현재 위치에서 목표 위치까지 일정 속도로 이동
            _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, cameraMovePoint, speed * Time.unscaledDeltaTime);
            distance = Vector3.Distance(_camera.transform.localPosition, cameraMovePoint);
            yield return null;
        }

        if (_cameraChange)
        {
            Main.Game.HiddenOBJSet();
            //subCam.enabled = _cameraChange;
            //mainCam.enabled = !_cameraChange;

            _camera.SetActive(!_cameraChange);
            _enabledRotate = true;
            _SubcamCanvas.SetActive(true);
            mainCam.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Dummy") | 1 << LayerMask.NameToLayer("MainHiddenOBJ"));
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("CameraHiddenOBJ");
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Monster");
            //카메라를 들었음
            _interactController.SetInteractDistance(Mathf.Infinity);
        }
        //Main.Game.OnCamera = _cameraChange;

        //Main.Game.HiddenOBJSet();
        _enabled = false; // 도착하면 이동을 멈춥니다.
    }
    public bool isCameraOn()
    {
        return _cameraChange;
    }
    public Transform GetCamTransform()
    {
        return mainCam.transform;
    }
}
