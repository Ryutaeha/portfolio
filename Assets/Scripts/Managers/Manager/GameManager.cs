using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
public enum GameState
{
    PLAY,
    PAUSE,
}
public class GameManager
{
    private GameObject _player;
    public GameObject Player { get { 
            if (_player == null)
            {
                _player = GameObject.FindWithTag("Player");
            }
            return _player;
        } }
    private ServiceLocator _serviceLocator;
    private float _originalTimeScale;
    public GameState CurState { get { return _gameState; } }
    GameState _gameState = GameState.PLAY;

    private bool _onCamera;
    public bool OnCamera { get { return _onCamera ; } set { _onCamera = value; } }

    public delegate void LanguageChange(int value);
    public event LanguageChange OnChangeLanguage;

    public delegate void NarrationPlaying(int strartCount, int endCount);
    public event NarrationPlaying OnNarrationPlaying;



    public void HandleLaguageChange(int value)
    {
        OnChangeLanguage?.Invoke(value);
    }

    public void HandleNarrationPlaying(int startCount,int endCount)
    {
        OnNarrationPlaying?.Invoke(startCount, endCount);
    }

    public event Action HiddenOBJ;


    public void PauseGame()
    {
        InteractController ic = Player.GetComponent<InteractController>();
        ic.CancelRewind();
        ic.GetComponent<PlayerInput>().enabled = false;
        _gameState = GameState.PAUSE;
        Cursor.lockState = CursorLockMode.None;
        _originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        Main.UI.ShowPopupUI<UI_Popup_Pause>();
        Player.GetComponent<CharacterController>().enabled = false;
    }
    public void ContinueGame()
    {
        Main.UI.ClosePopupUI();
        if (!Main.UI._isAllClosed)
        {
            return;
        }

        Time.timeScale = _originalTimeScale;
        Cursor.lockState = CursorLockMode.Locked;

        Player.GetComponent<PlayerInput>().enabled = true;
        Player.GetComponent<CharacterController>().enabled = true;
        _gameState = GameState.PLAY;
    }
    public void ClearGame()
    {
        _gameState = GameState.PAUSE;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.0083f;
        Main.StopAllCoroutine();
        Player.GetComponent<CharacterController>().enabled = false;
    }
    public void StartGame()
    {
        Main.Game.HiddenOBJ = null;
        _gameState = GameState.PLAY;
    }
    public ServiceLocator ServiceLocator
    {
        get
        {
            if (_serviceLocator == null)
            {
                // ServiceLocator 인스턴스 생성


                _serviceLocator = new ServiceLocator();

                SetServiceLocator();
            }

            return _serviceLocator;
        }
    }

    

    private void SetServiceLocator()
    {
        // Binder 인스턴스 생성 및 등록
        Binder binder = new();
        _serviceLocator.RegisterService(binder);
    }
    public void HiddenOBJSet()
    {
        HiddenOBJ?.Invoke();
    }
}