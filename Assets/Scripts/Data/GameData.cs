using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 인게임에서 사용할 데이터
/// </summary>
public class GameData
{
    #region Player Info

    public float MouseSensitivity = -1.0f;
    public InputActionAsset InputActions;

    //실시간 볼륨
    public float MasterVolume = 1f;
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;
    public float NarrationVolume = 1f;
    //백업 볼륨
    public float backUpMasterVolume = 1f;    
    public float backUpMusicVolume = 1f;
    public float backUpSFXVolume = 1f;
    public float backUpNarrationVolume = 1f;


    #endregion

}
