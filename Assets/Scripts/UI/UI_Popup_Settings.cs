using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputBinding;
using UnityEditor.Rendering;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_Popup_Settings : UI_Popup
{
    #region enums
    private enum Buttons
    {
        Btn_Audio,
        Btn_Video,
        Btn_Controls,
        Btn_Languages,
        Btn_Reset,
        Btn_OK,
        Btn_Cancel,
        Btn_Apply,
        Btn_ResetAudio,
        Btn_ResetVideo,
        Btn_ResetControls,
        Btn_Inverse,
    }
    private enum Sliders
    {
        MasterVolumeSlider,
        MusicVolumeSlider,
        SFXVolumeSlider,
        NarrationVolumeSlider,
        MouseSensitivity,
        FOV,
    }
    private enum Texts
    {
        Key_Forward,
        Key_Left,
        Key_Backward,
        Key_Right,
        Key_Jump,
        Key_Crouch,
        Key_Interact,
        Key_Stop,
        Key_Slow,
        Key_Ability
    }

    private enum InputFieldVolumes
    {
        MasterInputField,
        MusicInputField,
        SFXInputField,
        NarrationInputField,
    }
    private enum InputFieldValues
    {
        MouseSensitivityInputField
    }

    private enum DropDowns
    {
        ResolutionDropDown,
        LanguagesDropDown,
    }
    #endregion

    #region fields
    [SerializeField] private GameObject _panelAudio;
    [SerializeField] private GameObject _panelVideo;
    [SerializeField] private GameObject _panelControls;
    [SerializeField] private GameObject _panelLanguages;
    [SerializeField] private GameObject _lineAudio;
    [SerializeField] private GameObject _lineVideo;
    [SerializeField] private GameObject _lineControls;
    [SerializeField] private GameObject _lineLanguages;
    [SerializeField] private TMP_Text _InverseText;
    [SerializeField] private List<UI_KeyRebinding> _RebindList = new List<UI_KeyRebinding>();
    [SerializeField] private GameObject _narration;
    public AudioMixer _audioMixer;
    private bool _fullScreen;
    //public TextAsset json;
    //InputSystem Data

    private int languageIndex;
    #endregion

    #region initialize
    protected override bool Init()
    {
        if (!base.Init()) return false;
        SetUI<Button>();
        SetUI<Slider>();
        SetUI<TMP_Text>();
        SetUI<TMP_InputField>();
        SetUI<TMP_Dropdown>();
        SetBtnEvent();
        InitSettings();
        return true;
    }

    private void InitRebinding()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            Main.Data.Actions.LoadBindingOverridesFromJson(rebinds);

        foreach (UI_KeyRebinding keyrebind in _RebindList)
            keyrebind.ShowBindText();
    }

    private void InitSettings()
    {
        //MouseSensitivity 초기화
        GetUI<Slider>(Sliders.MouseSensitivity.ToString()).value = Remap(Main.Data.MouseSense, 0, 15, 0, 100);

        //Volume 초기화
        GetUI<Slider>(Sliders.MasterVolumeSlider.ToString()).value = Main.Sound.MasterVolume;
        GetUI<Slider>(Sliders.MusicVolumeSlider.ToString()).value = Main.Sound.MusicVolume;
        GetUI<Slider>(Sliders.SFXVolumeSlider.ToString()).value = Main.Sound.SFXVolume;
        GetUI<Slider>(Sliders.NarrationVolumeSlider.ToString()).value = Main.Sound.NarrationVolume;


        //해상도
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            _InverseText.text = "ON";
            _fullScreen = true;
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            _InverseText.text = "OFF";
            _fullScreen = false;
            PlayerPrefs.SetInt("FullScreen", 0);
        }
        GetUI<TMP_Dropdown>(DropDowns.ResolutionDropDown.ToString()).value = PlayerPrefs.GetInt("Resolution");

        foreach (InputFieldVolumes inputField in Enum.GetValues(typeof(InputFieldVolumes)))
        {
            TMP_InputField field = GetUI<TMP_InputField>(inputField.ToString());
            Sliders sliders = (Sliders)(int)inputField;
            field.onValueChanged.AddListener((newValue) => OnVolumeValueChanged(newValue, sliders, field));
        }

        GetUI<TMP_InputField>(InputFieldValues.MouseSensitivityInputField.ToString()).onValueChanged.AddListener(OnMouseSensitivityChanged);

        //언어 설정 가져오기
        LanguageSetting();
    }
    #endregion

    #region monobehaviour
    private void Update()
    {
        //Volume 적용
        float masterVolume = GetUI<Slider>(Sliders.MasterVolumeSlider.ToString()).value;
        Main.Sound.MasterVolume = masterVolume;
        SetAudioVolume("Master", masterVolume);

        float musicVolume = GetUI<Slider>(Sliders.MusicVolumeSlider.ToString()).value;
        Main.Sound.MusicVolume = musicVolume;
        SetAudioVolume("Music", musicVolume);

        float SFXVolume = GetUI<Slider>(Sliders.SFXVolumeSlider.ToString()).value;
        Main.Sound.SFXVolume = SFXVolume;
        SetAudioVolume("SFX", SFXVolume);

        float narrationVolume = GetUI<Slider>(Sliders.NarrationVolumeSlider.ToString()).value;
        Main.Sound.NarrationVolume = narrationVolume;
        SetAudioVolume("Narration", narrationVolume);

        //Slider 설정시 InputFieldText 동기화
        float masterVolumeToText = Mathf.Round(2.5f * masterVolume + 100f);
        GetUI<TMP_InputField>(InputFieldVolumes.MasterInputField.ToString()).text = masterVolumeToText.ToString();
        float musicVolumeToText = Mathf.Round(2.5f * musicVolume + 100f);
        GetUI<TMP_InputField>(InputFieldVolumes.MusicInputField.ToString()).text = musicVolumeToText.ToString();
        float sfxVolumeToText = Mathf.Round(2.5f * SFXVolume + 100f);
        GetUI<TMP_InputField>(InputFieldVolumes.SFXInputField.ToString()).text = sfxVolumeToText.ToString();
        float narrationVolumeToText = Mathf.Round(2.5f * narrationVolume + 100f);
        GetUI<TMP_InputField>(InputFieldVolumes.NarrationInputField.ToString()).text = narrationVolumeToText.ToString();

        float mouseSlider = GetUI<Slider>(Sliders.MouseSensitivity.ToString()).value;
        float mouseSense = Mathf.Round(mouseSlider);
        GetUI<TMP_InputField>(InputFieldValues.MouseSensitivityInputField.ToString()).text = mouseSense.ToString();

    }

    #endregion
    protected override void SetBtnEvent()
    {
        //탭 변경 버튼
        GetUI<Button>(Buttons.Btn_Audio.ToString()).onClick.AddListener(() =>
        {
            print("오디오 탭 버튼");
            _panelVideo.SetActive(false);
            _lineVideo.SetActive(false);
            _panelControls.SetActive(false);
            _lineControls.SetActive(false);
            _panelLanguages.SetActive(false);
            _lineLanguages.SetActive(false);
            _panelAudio.SetActive(true);
            _lineAudio.SetActive(true);

        });
        GetUI<Button>(Buttons.Btn_Video.ToString()).onClick.AddListener(() =>
        {
            print("비디오 탭 버튼");
            _panelAudio.SetActive(false);
            _lineAudio.SetActive(false);
            _panelControls.SetActive(false);
            _lineControls.SetActive(false);
            _panelLanguages.SetActive(false);
            _lineLanguages.SetActive(false);
            _panelVideo.SetActive(true);
            _lineVideo.SetActive(true);
        });
        GetUI<Button>(Buttons.Btn_Controls.ToString()).onClick.AddListener(() =>
        {
            print("컨트롤 탭 버튼");
            InitRebinding();
            _panelAudio.SetActive(false);
            _lineAudio.SetActive(false);
            _panelVideo.SetActive(false);
            _lineVideo.SetActive(false);
            _panelLanguages.SetActive(false);
            _lineLanguages.SetActive(false);
            _panelControls.SetActive(true);
            _lineControls.SetActive(true);
        });
        GetUI<Button>(Buttons.Btn_Languages.ToString()).onClick.AddListener(() =>
        {
            print("언어 탭 버튼");
            _panelAudio.SetActive(false);
            _lineAudio.SetActive(false);
            _panelControls.SetActive(false);
            _lineControls.SetActive(false);
            _panelVideo.SetActive(false);
            _lineVideo.SetActive(false);
            _panelLanguages.SetActive(true);
            _lineLanguages.SetActive(true);
            LanguageSetting();
        });

        //설정 적용/취소 버튼
        GetUI<Button>(Buttons.Btn_Reset.ToString()).onClick.AddListener(() =>
        {
            print("리셋 버튼");
            ResetVolume(); //볼륨 초기화
            ResetKeyBindings(); //키 변경 초기화
        });
        GetUI<Button>(Buttons.Btn_OK.ToString()).onClick.AddListener(() =>
        {
            print("OK 버튼");
            //TODO: 가기전에 적용버튼 누른것과 같은 동작하고 가기
            ApplySetting();
            Main.Scenes.CurrentScene.CurSceneUI.gameObject.SetActive(true);
            Main.UI.ClosePopupUI();
        });
        GetUI<Button>(Buttons.Btn_Cancel.ToString()).onClick.AddListener(() =>
        {
            print("Cancel 버튼");
            Main.Scenes.CurrentScene.CurSceneUI.gameObject.SetActive(true);
            Main.UI.ClosePopupUI();
            CancelSetting();
        });
        GetUI<Button>(Buttons.Btn_Apply.ToString()).onClick.AddListener(() =>
        {
            print("Apply 버튼");
            ApplySetting();
        });
        GetUI<Button>(Buttons.Btn_ResetAudio.ToString()).onClick.AddListener(() =>
        {
            print("Audio Reset 버튼");
            ResetVolume(); //볼륨 초기화
        });
        GetUI<Button>(Buttons.Btn_ResetVideo.ToString()).onClick.AddListener(() =>
        {
            print("Video Reset 버튼");
        });
        GetUI<Button>(Buttons.Btn_ResetControls.ToString()).onClick.AddListener(() =>
        {
            print("Control Reset 버튼");
            ResetKeyBindings();
        });
        GetUI<Button>(Buttons.Btn_Inverse.ToString()).onClick.AddListener(() =>
        {
            InvertScreen();
            print("Inverse 버튼");
        });

    }

    private void LanguageSetting()
    {
        //언어 설정 가져오기
        int language = PlayerPrefs.GetInt("Language");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
        GetUI<TMP_Dropdown>(DropDowns.LanguagesDropDown.ToString()).value = language;
        GetUI<TMP_Dropdown>(DropDowns.LanguagesDropDown.ToString()).RefreshShownValue();

    }
    public void InvertScreen()
    {
        if (PlayerPrefs.GetInt("FullScreen") == 0)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
            Screen.SetResolution(Screen.width, Screen.height, true);
            _fullScreen = true;
            _InverseText.GetComponent<TMP_Text>().text = "ON";
        }
        else if (PlayerPrefs.GetInt("FullScreen") == 1)
        {
            PlayerPrefs.SetInt("FullScreen", 0);
            Screen.SetResolution(Screen.width, Screen.height, false);
            _fullScreen = false;
            _InverseText.GetComponent<TMP_Text>().text = "OFF";
        }
    }

    public void ApplySetting()
    {
        //MouseSensitivity 적용
        float mouseSense = GetUI<Slider>(Sliders.MouseSensitivity.ToString()).value;
        mouseSense = Remap(mouseSense, 0, 100, 0, 15);

        Main.Data.MouseSense = mouseSense;
        GameObject player = Main.Game.Player;
        if (player != null)
        {
            player.GetComponent<PlayerEventController>().SetMouseSense();
        }

        ApplyVolumeSetting();
        ApplyResolution();
        //KeyBinding 저장
        var rebinds = Main.Data.Actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void UserLocalization(int index)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("Language", index);
        Main.Game.HandleLaguageChange(index);
    }

    public void ResetKeyBindings()
    {
        foreach (InputActionMap map in Main.Data.Actions.actionMaps)
            map.RemoveAllBindingOverrides();

        foreach (UI_KeyRebinding keyrebind in _RebindList)
            keyrebind.ShowBindText();

        PlayerPrefs.DeleteKey("rebinds");
    }

    public void CancelSetting()
    {
        //키 바인딩 취소
        //var rebinds = PlayerPrefs.GetString("rebinds");
        //if (!string.IsNullOrEmpty(rebinds))
        //    Main.Game.Actions.LoadBindingOverridesFromJson(rebinds);

        CancleVolumeSetting();
    }


    #region VolumeSetting
    private void ApplyVolumeSetting()
    {
        // 볼륨 백업 값 변경
        Main.Sound.Data.backUpMasterVolume = Main.Sound.MasterVolume;
        Main.Sound.Data.backUpMusicVolume = Main.Sound.MusicVolume;
        Main.Sound.Data.backUpSFXVolume = Main.Sound.SFXVolume;
        Main.Sound.Data.backUpNarrationVolume = Main.Sound.NarrationVolume;
    }

    private void CancleVolumeSetting()
    {
        //볼륨 백업값 가져오기
        Main.Sound.MasterVolume = Main.Sound.Data.backUpMasterVolume;
        Main.Sound.MusicVolume = Main.Sound.Data.backUpMusicVolume;
        Main.Sound.SFXVolume = Main.Sound.Data.backUpSFXVolume;
        Main.Sound.NarrationVolume = Main.Sound.Data.backUpNarrationVolume;

        //백업값으로 소리 변경
        SetVolumes();
    }
    private void SetAudioVolume(string name, float volume)
    {
        if (volume == -40f)
        {
            _audioMixer.SetFloat(name, -80);
        }
        else
        {
            _audioMixer.SetFloat(name, volume);
        }
    }

    private void ResetVolume()
    {
        Main.Sound.Data.backUpMasterVolume = -20f;
        Main.Sound.Data.backUpMusicVolume = -20f;
        Main.Sound.Data.backUpSFXVolume = -20f;
        Main.Sound.Data.backUpNarrationVolume = -20f;

        Main.Sound.MasterVolume = -20f;
        Main.Sound.MusicVolume = -20f;
        Main.Sound.SFXVolume = -20f;
        Main.Sound.NarrationVolume = -20f;

        //백업값으로 소리 변경
        SetVolumes();

        //백업값으로 Slider 변경
        GetUI<Slider>(Sliders.MasterVolumeSlider.ToString()).value = Main.Sound.MasterVolume;
        GetUI<Slider>(Sliders.MusicVolumeSlider.ToString()).value = Main.Sound.MusicVolume;
        GetUI<Slider>(Sliders.SFXVolumeSlider.ToString()).value = Main.Sound.SFXVolume;
        GetUI<Slider>(Sliders.NarrationVolumeSlider.ToString()).value = Main.Sound.NarrationVolume;
    }

    private void SetVolumes()
    {
        SetAudioVolume("Master", Main.Sound.MasterVolume);
        SetAudioVolume("Music", Main.Sound.MusicVolume);
        SetAudioVolume("SFX", Main.Sound.SFXVolume);
        SetAudioVolume("Narration", Main.Sound.NarrationVolume);
    }


    private void OnVolumeValueChanged(string newValue, Sliders sliderType, TMP_InputField inputField)
    {
        if (newValue != null)
        {
            // 입력된 값이 숫자인지 확인
            if (!float.TryParse(newValue, out float value))
            {
                // 입력된 값이 숫자가 아닌 경우, 이전 값으로 되돌림
                inputField.text = "0";
                return;
            }
            // 입력된 값이 범위를 벗어나는 경우, 범위 내로 값을 조정
            value = Mathf.Clamp(value, 0, 100);

            if (value != float.Parse(newValue))
            {
                inputField.text = value.ToString();
            }

            if (newValue == "")
            {
                newValue = "0";
                inputField.text = "0";
                GetUI<Slider>(sliderType.ToString()).value = 0;
            }

            //입력된 값을 slider에 적용
            float floatValue = Mathf.Round((0.4f * float.Parse(newValue) - 40f) * 10f) * 0.1f; //slider value값인 -40 ~ 0으로 변경
            GetUI<Slider>(sliderType.ToString()).value = floatValue;
        }
    }
    #endregion

    private void OnMouseSensitivityChanged(string newValue)
    {
        if (newValue != null)
        {
            // 입력된 값이 숫자인지 확인
            if (!float.TryParse(newValue, out float value))
            {
                // 입력된 값이 숫자가 아닌 경우, 이전 값으로 되돌림
                GetUI<TMP_InputField>(InputFieldValues.MouseSensitivityInputField.ToString()).text = "0";
                return;
            }
            // 입력된 값이 범위를 벗어나는 경우, 범위 내로 값을 조정
            value = Mathf.Clamp(value, 0, 100);

            if (value != float.Parse(newValue))
            {
                GetUI<TMP_InputField>(InputFieldValues.MouseSensitivityInputField.ToString()).text = value.ToString();
            }

            if (newValue == "")
            {
                GetUI<TMP_InputField>(InputFieldValues.MouseSensitivityInputField.ToString()).text = "0";
            }

            //입력된 값을 slider에 적용
            float floatValue = Mathf.Round(float.Parse(newValue));
            GetUI<Slider>(Sliders.MouseSensitivity.ToString()).value = floatValue;
        }
    }

    private void ApplyResolution()
    {
        int idx = GetUI<TMP_Dropdown>(DropDowns.ResolutionDropDown.ToString()).value;
        PlayerPrefs.SetInt("Resolution", idx);
        Main.Scenes.SetResolution();
    }
    public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
    }
}
