using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{    
    protected UI_GameScene _gameSceneUI;
    protected Stack<FrameData> recordedFrames = new Stack<FrameData>();
    protected bool isRecording = false;
    protected bool isPlayingBack = false;
    [HideInInspector] public bool isClick;
    [HideInInspector]
    public bool isDone = false;
    protected Renderer objectRenderer;
    protected Rigidbody objectRigidbody;
    protected Coroutine playbackCoroutine;
    [SerializeField] protected ObjectPropertyType type;
    public ObjectPropertyType Type => type;

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
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

        if (!isClick && isPlayingBack&&type != ObjectPropertyType.NORMAL)
        {
            StopPlaybackAndClearFrames();
            if (_gameSceneUI == null)
                _gameSceneUI = Main.Scenes.CurrentScene.CurSceneUI as UI_GameScene;
        }
    }

    public virtual void Recording(bool newIsDone, bool newIsClick)
    {
        isDone = newIsDone;
        isClick = newIsClick;
    }

    protected virtual void StartRecording()
    {
        isRecording = true;
        StartCoroutine(RecordFrameCoroutine());
    }

    protected virtual void StopRecording()
    {
        isRecording = false;
        StopCoroutine(RecordFrameCoroutine());
    }

    protected virtual void StartReversePlayback()
    {
        if (type != ObjectPropertyType.NORMAL)
        {
            objectRigidbody.isKinematic = true;
        }
        isPlayingBack = true;
        playbackCoroutine = StartCoroutine(PlaybackCoroutine());
    }


    public virtual void StopPlaybackAndClearFrames()
    {
        StopCoroutine(playbackCoroutine);
        recordedFrames.Clear();
        isPlayingBack = false;
        objectRigidbody.isKinematic = false;
    }

    private IEnumerator RecordFrameCoroutine()
    {
        while (isRecording)
        {
            RecordCurrentFrame();
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected virtual IEnumerator PlaybackCoroutine()
    {
        Queue<FrameData> playbackStack = new Queue<FrameData>(recordedFrames);
        while (playbackStack.Count > 0 && isPlayingBack)
        {
            FrameData frame = playbackStack.Dequeue();
            yield return StartCoroutine(InterpolateFrame(frame, 0.1f));
        }
        Recording(true, true);
        recordedFrames.Clear();
        playbackStack.Clear();
      
    }



    protected IEnumerator InterpolateFrame(FrameData frame, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;
        //Color startColor = objectRenderer.material.color;
        float elapsedTime = 0;
        if (type == ObjectPropertyType.NORMAL)
        {
            StopPlaybackAndClearFrames();
        }
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, frame.position, t);
            transform.rotation = Quaternion.Lerp(startRotation, frame.rotation, t);
            transform.localScale = Vector3.Lerp(startScale, frame.scale, t);
            //objectRenderer.material.color = Color.Lerp(startColor, frame.color, t);
            yield return null;
        }
    }

    private void RecordCurrentFrame()
    {
        FrameData frame = new FrameData
        {
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale,
            //color = objectRenderer.material.color
        };
        recordedFrames.Push(frame);
    }

}

public struct FrameData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    //public Color color;
}
