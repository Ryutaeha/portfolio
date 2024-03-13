using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPropertyType
{
    RESTORE,
    DESTROY,
    NORMAL,
    NONE
}

public class DestroyedObject : ReplayRecorder
{
    public delegate void StateChangeHandler(DestroyedObject sender, bool isDone, bool isClick);
    public event StateChangeHandler OnStateChange;
    public bool isDissolve;
    [SerializeField] private float speed;
    private Break _break;
    public Renderer _renderer;
    private void OnEnable()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (isDissolve)
            StartCoroutine(CoDissolve());
    }
    void Start()
    {
        base.Start(); // 부모 클래스의 Start 메서드 호출
        _break = transform.parent.gameObject.GetComponent<Break>();
        transform.parent.GetComponent<ParentObject>().childStates.Add(this, (false, false));

    }

    protected override void Update()
    {
        if (Type == ObjectPropertyType.NONE)
        {
            return;
        }
        if (objectRigidbody && !objectRigidbody.IsSleeping())
        {
            if (!isRecording && !isPlayingBack)
            {
                StartRecording();
            }
        }
        else if (isRecording)
        {
            StopRecording();
        }

        if (isClick && !isPlayingBack)
        {
            StartReversePlayback();
            if (_gameSceneUI == null)
                _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;
        }
    }


    public override void Recording(bool newIsDone, bool newIsClick)
    {
        base.Recording(newIsDone, newIsClick);
        OnStateChange?.Invoke(this, newIsDone, newIsClick);
    }

    protected override void StartRecording()
    {
        base.StartRecording(); // 부모 클래스의 StartRecording 메서드 호출
    }

    protected override void StopRecording()
    {
        base.StopRecording(); // 부모 클래스의 StopRecording 메서드 호출
    }

    protected override void StartReversePlayback()
    {
        objectRigidbody.isKinematic = true;
        isPlayingBack = true;
        playbackCoroutine = StartCoroutine(PlaybackCoroutine()); // 부모 클래스의 StartReversePlayback 메서드 호출
    }

    public override void StopPlaybackAndClearFrames()
    {
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }
        isPlayingBack = false;
    }

    protected override IEnumerator PlaybackCoroutine()
    {
        Queue<FrameData> playbackStack = new Queue<FrameData>(recordedFrames);
        while (playbackStack.Count > 0 && isPlayingBack)
        {
            FrameData frame = playbackStack.Dequeue();
            yield return StartCoroutine(InterpolateFrame(frame, speed));
        }
        Recording(true, true);
        recordedFrames.Clear();
        playbackStack.Clear();
    }

    public void Destroying()
    {
        _break.Destroying();
    }
    public void Restoring()
    {
        _break.Restoring();
    }
    IEnumerator CoDissolve()
    {
        float alpha = 2.0f;
        while (alpha >= 0)
        {
            _renderer.materials[0].SetFloat("_Power", alpha);
            _renderer.materials[1].SetFloat("_Power", alpha);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            alpha -= Time.unscaledDeltaTime;
        }
        gameObject.SetActive(false);
    }
}
