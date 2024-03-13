using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTest : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera subCam;
    [SerializeField] private Vector3 _position1 = new (0.35f,-0.01f,0.05f);
    [SerializeField] private Vector3 _position2 = new(1, -0.5f, 1f);
    [SerializeField] private float rotateSpeed = 3f;  // 회전 속도
    private IEnumerator rotateCoroutine;
    private float speed = 5.0f; // 이동 속도
    private bool _cameraChenge;
    private bool _enabled = false;
    private bool _enabledRotate = false;
    private void Start()
    {
        _cameraChenge = Main.Game.OnCamera;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Debug.Log("G 키가 눌렸습니다.");
            CameraMoveCheck();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 키가 눌렸습니다.");
            RotationCam(90);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotationCam(-90);
        }
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
        if(_cameraChenge && _enabledRotate)
        {
            rotateCoroutine = RotateObjectCoroutine(rot);
            StartCoroutine(rotateCoroutine);
        }
    }
    private IEnumerator RotateObjectCoroutine(float rot)
    {
        _enabledRotate = false;
        Quaternion startRotation = subCam.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(Vector3.forward * rot);
        float timeElapsed = 0;

        while (timeElapsed < rotateSpeed)
        {
            subCam.transform.rotation = Quaternion.RotateTowards(subCam.transform.rotation, endRotation, Time.deltaTime * 90 / rotateSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        subCam.transform.rotation = endRotation;
        _enabledRotate = true;
        rotateCoroutine = null;
    }
    private IEnumerator CameraMoveCoroutine()
    {
        Vector3 cameraMovePoint = _cameraChenge ? _position2 : _position1;

        // 현재 위치와 목표 위치 사이의 거리 계산
        float distance = Vector3.Distance(_camera.transform.localPosition, cameraMovePoint);

        _cameraChenge = cameraMovePoint == _position1 ? true : false;
        if (!_cameraChenge)
        {
            if(rotateCoroutine != null)
                StopCoroutine(rotateCoroutine);

            //해당 오브젝트의 로테이션값을 0,0,0으로 변경하고 싶다.
            subCam.transform.localEulerAngles = Vector3.zero;
            subCam.enabled = _cameraChenge;
            mainCam.enabled = !_cameraChenge;
            _camera.SetActive(!_cameraChenge);
        }
        // 거리가 충분히 작으면 도착한 것으로 간주
        while (distance >= 0.001f)
        {
            // 현재 위치에서 목표 위치까지 일정 속도로 이동
            _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, cameraMovePoint, speed * Time.deltaTime);
            distance = Vector3.Distance(_camera.transform.localPosition, cameraMovePoint);
            yield return null;
        }

        if (_cameraChenge)
        {
            subCam.enabled = _cameraChenge;
            mainCam.enabled = !_cameraChenge;

            _camera.SetActive(!_cameraChenge);
            _enabledRotate = true;
        }
        Debug.Log(_cameraChenge);
        _enabled = false; // 도착하면 이동을 멈춥니다.
    }
    


}
