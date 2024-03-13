using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager
{
    //시각 효과
    private Volume _volume;
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;

    //코루틴
    private Coroutine _coSaturationChangeCoroutine;
    private Coroutine _coIntensityChangeCoroutine;
    private Coroutine _coTimeScaleChangeCoroutine;

    //
    private float _targetSaturation;
    private float _targetVignetteIntensity;
    private float _targetTimeScale;
    private float _changeSpeed = 12.0f;

    public void SetTimeStop()
    {
        SetTimeScale(0.0001f);
        SetSaturationSmooth(-100f);
    }
    public void SetTimeSlow()
    {
        SetTimeScale(0.1f);
        SetVignetteIntensitySmooth(0.5f);
    }
    public void SetTimePlay()
    {
        SetTimeScale(1.0f);
        SetSaturationSmooth(24f);
        SetVignetteIntensitySmooth(0f);
    }

    /// <summary>
    /// 시간 정지일 때 화면이 흑백으로 만드는 메서드입니다.
    /// </summary>
    public void SetSaturationSmooth(float targetSaturation)
    {
        if (_coSaturationChangeCoroutine != null)
        {
            Main.StopCoroutine(_coSaturationChangeCoroutine);
        }
        if (_volume == null)
            _volume = GameObject.FindWithTag("PostProcessing").GetComponent<Volume>();
        _volume.profile.TryGet(out _colorAdjustments);

        this._targetSaturation = targetSaturation;

        if(_colorAdjustments!=null)
            _coSaturationChangeCoroutine = Main.StartCoroutine(CoSmoothSaturationChange());
    }
    /// <summary>
    /// 슬로우 모션일 때 화면의 테두리를 어둡게 만드는 메서드입니다.
    /// </summary>
    public void SetVignetteIntensitySmooth(float targetIntensity)
    {
        if (_coIntensityChangeCoroutine != null)
        {
            Main.StopCoroutine(_coIntensityChangeCoroutine);
        }
        if (_volume == null)
            _volume = GameObject.FindWithTag("PostProcessing").GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);

        this._targetVignetteIntensity = targetIntensity;

        if (_vignette != null)
            _coIntensityChangeCoroutine = Main.StartCoroutine(CoSmoothIntensityChange());
    }
    public void SetTimeScale(float targetTimeScale)
    {
        if(_coTimeScaleChangeCoroutine!= null)
        {
            Main.StopCoroutine(_coTimeScaleChangeCoroutine);
        }
        this._targetTimeScale = targetTimeScale;
        _coTimeScaleChangeCoroutine = Main.StartCoroutine(CoTimeScaleChange());
    }

    private IEnumerator CoSmoothSaturationChange()
    {
        while (Mathf.Abs(_colorAdjustments.saturation.value - _targetSaturation) > 0.1f)
        {
            if (Main.Game.CurState != GameState.PAUSE)
            {
                float currentSaturation = _colorAdjustments.saturation.value;
                float newSaturation = Mathf.Lerp(currentSaturation, _targetSaturation, Time.fixedUnscaledDeltaTime * _changeSpeed);
                _colorAdjustments.saturation.value = newSaturation;

                yield return new WaitForSecondsRealtime(0.02f);
            }
            else
                yield return null;
        }

        _colorAdjustments.saturation.value = _targetSaturation;
    }
    private IEnumerator CoSmoothIntensityChange()
    {
        while (Mathf.Abs(_vignette.intensity.value - _targetVignetteIntensity) > 0.01f)
        {
            if (Main.Game.CurState != GameState.PAUSE)
            {
                float currentIntensity = _vignette.intensity.value;
                float newIntensity = Mathf.Lerp(currentIntensity, _targetVignetteIntensity, Time.fixedUnscaledDeltaTime * _changeSpeed);
                _vignette.intensity.value = newIntensity;

                yield return new WaitForSecondsRealtime(0.02f);
            }
            else
                yield return null;
        }

        _vignette.intensity.value = _targetVignetteIntensity;
    }

    private IEnumerator CoTimeScaleChange()
    {
        while (Mathf.Abs(Time.timeScale - _targetTimeScale) > 0.001f)
        {
            if (Main.Game.CurState != GameState.PAUSE)
            {
                //Debug.Log("나 실행되고 있어");
                Time.timeScale = Mathf.Lerp(Time.timeScale, _targetTimeScale, Time.fixedUnscaledDeltaTime * _changeSpeed);
                Time.fixedDeltaTime = Time.timeScale * 0.0083f;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            else
                yield return null;
        }

        Time.timeScale = _targetTimeScale;
        //시간 정지 3초후 풀림, 슬로우는 해당 없음
        if (Time.timeScale < 0.0002f)
        {
            yield return new WaitForSecondsRealtime(3f);
            SetTimePlay();
            Main.Game.Player.GetComponent<PlayerEventController>().EndPaused();
        }
    }
}
