using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObjectPropertyType.NORMAL에 대한 특수 동작을 처리하는 ReplayRecorder의 파생 클래스
public class NormalReplayRecorder : ReplayRecorder
{
    protected override void Update()
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
    }

    protected override void StartReversePlayback()
    {
        objectRigidbody.isKinematic = type == ObjectPropertyType.NORMAL;
        isPlayingBack = true;
        playbackCoroutine = StartCoroutine(PlaybackCoroutine());
    }

    public override void StopPlaybackAndClearFrames()
    {
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
        }
        if (type == ObjectPropertyType.NORMAL)
        {
            recordedFrames.Clear(); // Clear frames only for NORMAL type after playback.
        }
        isPlayingBack = false;
        objectRigidbody.isKinematic = false;
    }

    protected override IEnumerator PlaybackCoroutine()
    {
        Queue<FrameData> playbackStack = new Queue<FrameData>(recordedFrames);
        while (playbackStack.Count > 0 && isPlayingBack)
        {
            FrameData frame = playbackStack.Dequeue();
            yield return StartCoroutine(InterpolateFrame(frame, 0.1f));
        }
        if (type == ObjectPropertyType.NORMAL)
        {
            Recording(true, false); // This ensures smooth finish for NORMAL type without interrupting.
            recordedFrames.Clear(); // Additionally clears frames only for NORMAL type.
            isPlayingBack = false;
            objectRigidbody.isKinematic = false;
        }
    }

    protected IEnumerator InterpolateFrame(FrameData frame, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, frame.position, t);
            transform.rotation = Quaternion.Lerp(startRotation, frame.rotation, t);
            transform.localScale = Vector3.Lerp(startScale, frame.scale, t);
            yield return null;
        }
    }

}
