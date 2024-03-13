using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Default;
    public UI_Scene CurSceneUI { get; protected set; }

    private bool _Initialized = false;
    void Awake()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        Cursor.lockState = CursorLockMode.Locked; //1인칭 화면에서 마우스 커서 숨기기

        if (Main.Resource.Loaded)
        {

            //Main.Data.Initialize();
            //Main.Game.Initialize();
            Initialize();
            
        }
        else
        {
            
            Main.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) => {
                //Debug.Log($"[GameScene] Load asset {key} ({count}/{totalCount})");
                if (count >= totalCount)
                {
                    Main.Resource.Loaded = true;
                    //Main.Data.Initialize();
                    //Main.Game.Initialize();
                    Initialize();
                }
            });

        }

    }

    public abstract void Clear();

    protected virtual bool Initialize()
    {
        if (_Initialized) return false;

        //Main.ScenesManager.CurrentScene = this;

        Main.Sound.InitializedSound();
        Object obj = FindObjectOfType<EventSystem>();
        if (obj == null)
            obj = Main.Resource.Instantiate("EventSystem.prefab");
        obj.name = "@EventSystem";
        DontDestroyOnLoad(obj);
        Main.Game.StartGame();
        Main.Scenes.SetResolution();
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked; //1인칭 화면에서 마우스 커서 숨기기
#endif
        _Initialized = true;
        return true;
    }

}
