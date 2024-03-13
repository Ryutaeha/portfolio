using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main _instance;
    private static bool _initialized;
    private static Main Instance
    {
        get
        {
            if (_initialized) return _instance;
            _initialized = true;
            GameObject main = GameObject.Find("@Main");
            if (main != null) return _instance;
            main = new GameObject { name = "@Main" };
            main.AddComponent<Main>();
            DontDestroyOnLoad(main);
            _instance = main.GetComponent<Main>();
            return _instance;
        }
    }
    #region Fields
    private readonly GameManager _gameManager = new();
    private readonly DataManager _dataManager = new();
    private readonly PoolManager _poolManager = new();
    private readonly ResourceManager _resourceManager = new();
    private readonly ScenesManager _scenesManager = new();
    private SoundManager _soundManager = new();
    private readonly TimeManager _timeManager = new();
    private readonly UIManager _uiManager = new();
    private DataManager.StageClear _stageClear = new();
    #endregion

    #region Properties
    public static string NextScene { get; set; }
    public static bool FirstLord { get; set; } = true;
    public static GameManager Game => Instance._gameManager;
    public static DataManager Data => Instance._dataManager;
    public static PoolManager Pool => Instance._poolManager;
    public static ResourceManager Resource => Instance._resourceManager;
    public static ScenesManager Scenes => Instance._scenesManager;
    public static SoundManager Sound
    {
        get => Instance._soundManager;
        set => Instance._soundManager = value;
    }
    public static TimeManager Time => Instance._timeManager;
    public static UIManager UI => Instance._uiManager;
    public static DataManager.StageClear StageClear
    {
        get => Instance._stageClear;
        set => Instance._stageClear = value;
    }
    #endregion


    #region CoroutineHelper

    public new static Coroutine StartCoroutine(IEnumerator coroutine) => (Instance as MonoBehaviour).StartCoroutine(coroutine);
    public new static void StopCoroutine(Coroutine coroutine) => (Instance as MonoBehaviour).StopCoroutine(coroutine);
    public static void StopAllCoroutine() => (Instance as MonoBehaviour).StopAllCoroutines();
    #endregion
}
