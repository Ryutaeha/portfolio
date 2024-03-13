using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public interface IDamageable
{
    void TakeDamage(int damage);
}
public class PlayerEventController : MonoBehaviour, IDamageable
{
    [HideInInspector] public CharacterController characterController;

    private float fallingVelocity;
    private float jumpVelocity;

    [Header("Movement")] //움직임
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public LayerMask AccelLayerMask;
    [HideInInspector] private float MoveSpeed = 5f;
    [HideInInspector] public float accelerate = 0f;

    [Header("Look")] // 시야
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float LookSensitivity;
    private Vector2 mouseDelta;
    private CameraController _camController;
    [HideInInspector] public bool canLook = true;
    private int rotateCount = 0;
    private int rotateInt = 0;

    [Header("Jump")] // 점프
    public float jumpForce;
    [HideInInspector] public bool Jump;
    //public bool IsJumpPad;

    [Header("Sit")] // 앉기
    private bool isDeath;
    Coroutine _coStandUp;
    [HideInInspector] public bool isSit;
    private const float sittingHeight = -0.5f;
    private const float sitTimeStep = 0.05f;

    [Header("Setting")]
    [SerializeField] private PlayerInput _playerInput;
    //public PlayerInput PlayerInput => playerInput;


    public LayerMask GlassLayerMask;


    [Header("TimeStop")] //시간 조작
    public GameObject Clock;
    private Animator clockAnimator;
    private AudioSource clockAudioSource;
    [HideInInspector] public bool isClockOpen = false;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public bool slow = false;
    private float orginFixeddelta;
    [SerializeField] private float pausedCoolTime = 10.0f;
    public float currentCoolTime = -1f;
    private UI_GameScene _gameScene;
    public bool noCooltime;

    [Header("CameraControl")] //카메라 화면 on/off
    //public GameObject OnRecordUI;

    //public Animator CamerAnimator;

    [Header("Stat")]
    private int HP = 100;
    public event Action OnTakeDamage;
    public event Action OnDeath;
    PlayerCheckPoint _playerCheckPoint;

    #region LifeCycle
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        clockAnimator = Clock.GetComponent<Animator>();
        clockAudioSource = Clock.GetComponent<AudioSource>();
        _camController = GetComponent<CameraController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerCheckPoint = GetComponent<PlayerCheckPoint>();
    }

    void Start()
    {
        orginFixeddelta = Time.fixedDeltaTime;
        SetMouseSense();

        //빌드 시 key rebinding 해결한 코드
        _playerInput.actions = Main.Data.Actions;
        _gameScene = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;        
    }

    private void Update()
    {
        if (clockAudioSource.pitch != Time.timeScale)
        {
            clockAudioSource.pitch = Time.timeScale;
        }

        if (Main.Game.CurState != GameState.PLAY)
            return;

        if (canLook)
        {
            CameraLook();
        }

        Move(); //기본 움직임

        GravityFalling(); //점프와 중력

        SitAnimation();

        AccelerateMove(); //가속 발판 처리
    }
    #endregion

    #region PlayerMoveMethod

    private void GravityFalling()
    {
        if (!IsGrounded()) //중력에 의한 추락
        {
            fallingVelocity -= 9.81f * Time.unscaledDeltaTime;
            float _velocity = fallingVelocity + jumpVelocity;
            characterController.Move(transform.up * _velocity * Time.unscaledDeltaTime);
        }
        else
        {
            //Debug.Log("ground");
            fallingVelocity = 0; // 지상에 있을 때 중력에 맞춰 y 속도를 초기화
            jumpVelocity = 0;
        }
    }

    private void Move() //기본 방향키 움직임
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        characterController.Move(dir * MoveSpeed * Time.unscaledDeltaTime);
    }

    private void SitAnimation() //부드러운 앉기
    {
        if (isDeath)
            return;
        if (isSit)
        {
            float targetY = Mathf.Max(cameraContainer.transform.localPosition.y - sitTimeStep, sittingHeight);
            cameraContainer.transform.localPosition = new Vector3(0, targetY, 0);
        }
        else
        {
            float targetY = Mathf.Min(cameraContainer.transform.localPosition.y + sitTimeStep, 0f);
            cameraContainer.transform.localPosition = new Vector3(0, targetY, 0);
        }
    }

    private void AccelerateMove() //가속 발판
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        characterController.Move(dir * accelerate * Time.unscaledDeltaTime);
    }

    private void CameraLook() //카메라를 마우스로 움직일 수 있게 하는 것.
    {
        rotateInt = (rotateCount) % 4;
        int reverse;

        //print(transform.eulerAngles.z);

        if (Mathf.Approximately(transform.eulerAngles.z, 180f))
            reverse = -1;
        else
            reverse = 1;

        if (rotateInt == 0)
        {
            //정방향
            camCurXRot += mouseDelta.y * LookSensitivity * Time.unscaledDeltaTime;
            camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

            transform.eulerAngles += new Vector3(0, mouseDelta.x * reverse * LookSensitivity * Time.unscaledDeltaTime, 0);
        }
        else if (rotateInt == 1 || rotateInt == -3)
        {
            //오른쪽(e)            
            transform.eulerAngles += new Vector3(0, mouseDelta.y * LookSensitivity * reverse * Time.unscaledDeltaTime, 0);

            camCurXRot -= mouseDelta.x * LookSensitivity * Time.unscaledDeltaTime;
            camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        }
        else if (rotateInt == 2 || rotateInt == -2)
        {
            //역방향
            camCurXRot -= mouseDelta.y * LookSensitivity * Time.unscaledDeltaTime;
            camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

            transform.eulerAngles += new Vector3(0, -mouseDelta.x * reverse * LookSensitivity * Time.unscaledDeltaTime, 0);
        }
        else if (rotateInt == -1 || rotateInt == 3)
        {
            //왼쪽(q)
            transform.eulerAngles += new Vector3(0, -mouseDelta.y * LookSensitivity * reverse * Time.unscaledDeltaTime, 0);

            camCurXRot += mouseDelta.x * LookSensitivity * Time.unscaledDeltaTime;
            camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        }
        else { Debug.Log("잘못된 회전값"); }
    }

    private IEnumerator JumpIsNotGround() //점프 Coroutine
    {
        if (Jump) //점프 시
        {
            jumpVelocity = jumpForce * 0.1f;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        Jump = false;
    }

    #endregion

    #region InputSystem

    #region Player
    public void OnLookInput(InputAction.CallbackContext context) //마우스 좌표 input
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context) // 움직임 input
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnSitInput(InputAction.CallbackContext context) // 앉기 
    {
        if (context.phase == InputActionPhase.Started && !isSit)
        {
            isSit = true;
            characterController.height = 1;
            characterController.center = new Vector3(0, -0.5f, 0);
        }
        else if (context.phase == InputActionPhase.Canceled && isSit)
        {
            if (_coStandUp != null)
                StopCoroutine(_coStandUp);
            _coStandUp = StartCoroutine(CoStandUp());
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context) // 점프
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            Jump = true;
            StartCoroutine(JumpIsNotGround());
        }
    }

    public void OnPauseButton(InputAction.CallbackContext context) // 게임 일시 정지
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (Main.Game.CurState == GameState.PLAY)
            {
                Main.Game.PauseGame();
            }
            else if (Main.Game.CurState == GameState.PAUSE)
            {
                Main.Game.ContinueGame();
            }
        }
    }
    #endregion

    #region Time

    public void ClockONOFF(InputAction.CallbackContext context) //시계 꺼내기
    {
        if (slow || paused)
            return;
        if (context.phase == InputActionPhase.Started)
        {
            if (isClockOpen) //시계가 열려있는 상태이면
            {
                clockAnimator.SetTrigger("Close");
                isClockOpen = false;
            }
            else
            {
                clockAnimator.SetTrigger("Open");
                isClockOpen = true;
            }
        }
    }


    public void OnTime(InputAction.CallbackContext context) //시간 정지
    {
        if (currentCoolTime > 0f)
        {
            print("쿨타임 중입니다.");
            return;
        }
        if (slow)
            return;
        if (!isClockOpen)
            return;

        if (context.phase == InputActionPhase.Started)
        {
            if (paused)
            {
                paused = false;
                if (Time.timeScale < 0.0002f)
                    StartCoroutine(CoSetStopCoolTime());
                Main.Time.SetTimePlay();
            }
            else
            {
                paused = true;
                Main.Time.SetTimeStop();
            }
        }
    }

    public void OnSlow(InputAction.CallbackContext context) // 슬로우 모션
    {
        if (paused)
            return;
        if (!isClockOpen)
            return;

        if (context.phase == InputActionPhase.Started)
        {
            if (slow)
            {
                slow = false;
                Main.Time.SetTimePlay();
            }
            else
            {
                slow = true;
                Main.Time.SetTimeSlow();
            }
        }
    }
    #endregion

    #region Recorder

    public void OnCamera(InputAction.CallbackContext context)
    {
        _camController.CameraMoveCheck();
        rotateCount = 0;
    }

    public void OnLeftRotate(InputAction.CallbackContext context) //카메라 왼쪽 회전
    {
        if (!_camController.isCameraOn() || context.phase != InputActionPhase.Started)
            return;
        _playerInput.enabled = false;
        _camController.RotationCam(90);
    }

    public void OnRightRotate(InputAction.CallbackContext context) //카메라 오른쪽 회전
    {
        if (!_camController.isCameraOn() || context.phase != InputActionPhase.Started)
            return;
        _playerInput.enabled = false;
        _camController.RotationCam(-90);
    }
    #endregion

    #endregion


    public bool IsGrounded() // 지면인지 파악하는 메서드
    {
        if (Jump)
        {
            return false;
        }

        Ray[] rays = new Ray[9]
        {
            new Ray(transform.position + (transform.up * 0.03f), -transform.up), //중앙
            new Ray(transform.position + (transform.forward * 0.5f) + (transform.up * 0.03f) , -transform.up), //앞
            new Ray(transform.position + (-transform.forward * 0.5f)+ (transform.up * 0.03f), -transform.up), //뒤
            new Ray(transform.position + (-transform.right * 0.5f) + (transform.up * 0.03f), -transform.up),//좌
            new Ray(transform.position + (transform.right * 0.5f) + (transform.up * 0.03f), -transform.up), //우

            //대각선 4방향 추가
            new Ray(transform.position + (transform.forward * 0.353f) + (transform.right * 0.353f)+ (transform.up * 0.03f) , -transform.up),
            new Ray(transform.position + (-transform.forward * 0.353f) + (transform.right * 0.353f)+ (transform.up * 0.03f) , -transform.up),
            new Ray(transform.position + (transform.forward * 0.353f) + (-transform.right * 0.353f)+ (transform.up * 0.03f) , -transform.up),
            new Ray(transform.position + (-transform.forward * 0.353f) + (-transform.right * 0.353f)+ (transform.up * 0.03f) , -transform.up),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsAccelerator()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1.2f, AccelLayerMask))
        {
            return true;
        }
        else return false;
    }

    public GameObject CheckGround() //바닥 체크
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, 1.1f, GlassLayerMask)) //바닥 유리일때 부수기
        {
            hit.collider.GetComponent<CheckObjectVelocity>().DestroyGlass();
        }

        if (Physics.Raycast(ray, out hit, 1.1f, groundLayerMask))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.5f), -transform.up * 1.03f);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.5f), -transform.up * 1.03f);
        Gizmos.DrawRay(transform.position + (transform.right * 0.5f), -transform.up * 1.03f);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.5f), -transform.up * 1.03f);
    }

    public void ToggleCursor(bool toggle) //커서 활성화/비활성화 메서드
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void SetRotateCount(int value)
    {
        rotateCount += value;
    }
    public void SetMouseSense() //마우스 민감도
    {
        LookSensitivity = Main.Data.MouseSense;
    }

    public void EndPaused() //일시정지 강제 해제
    {
        paused = false;
        Clock.GetComponent<AudioSource>().Play();
        StartCoroutine(CoSetStopCoolTime());
    }

    private IEnumerator CoSetStopCoolTime() //일시정지 쿨타임
    {
        if (noCooltime)
            yield break;
        currentCoolTime = pausedCoolTime;
        _gameScene.SetCoolTimeImage(pausedCoolTime);
        while (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        currentCoolTime = 0f;
    }

    public void TakeDamage(int damage)
    {
        if (HP <= 0)
            return;

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            cameraContainer.transform.localPosition = new Vector3(0, -0.8f, 0);
            StartCoroutine(CoDeath());
        }

        OnTakeDamage?.Invoke();
    }
    private IEnumerator CoDeath()
    {
        if (_camController.isCameraOn())
        {
            _camController.CameraMoveCheck();
            rotateCount = 0;
        }

        isDeath = true;
        _playerInput.enabled = false;
        characterController.enabled = false;
        yield return new WaitForSecondsRealtime(3.0f);
        cameraContainer.transform.localPosition = new Vector3(0, 0f, 0);
        OnDeath?.Invoke();
        _playerCheckPoint.ReSpawn();
        _playerInput.enabled = true;
        characterController.enabled = true;
        isDeath = false;
        HP = 100;
    }
    private IEnumerator CoStandUp()
    {
        Ray ray = new Ray(transform.position, transform.up);
        while (Physics.Raycast(transform.position, Vector3.up,out RaycastHit hit,1f))
        {

            print(hit.rigidbody.gameObject.name);
            yield return null;
        }

        characterController.height = 2;
        characterController.center = new Vector3(0, 0f, 0);
        isSit = false;
    }
}