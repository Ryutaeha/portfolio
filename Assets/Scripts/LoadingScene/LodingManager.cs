using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingManager : BaseScene
{

    private int _count;
    private int _totalCount;
    private string _key;
    private bool _resourcesLoaded = false;
    [SerializeField] private Image progressBar; 
    [SerializeField] private GameObject anyKeyDownTxt;
    private float timer = 0.0f; // 페이크 로딩을 계산하기 위한 타이머 변수
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        Main.NextScene ??= "TitleScene";

        //  Main.Sound.PlayBGM("BGM");
        Main.StageClear = Main.Data.Lord();
        LoadResourcesAndScene();
        return true;
    }

    private void LoadResourcesAndScene()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        string loadName;
        //loadName = Main.NextScene == "TitleScene" ? Main.NextScene : "GameScene";
        loadName = LoadSceneResource();
        Main.Resource.LoadAllAsync<Object>($"{loadName}", (key, count, totalCount) =>
        {
            _key = key;
            _count = count;
            _totalCount = totalCount;
            Debug.Log($"[{Main.NextScene}] Load asset {_key} ({_count}/{_totalCount})");
            if (_count != _totalCount) return;
            _resourcesLoaded = true; // 리소스 로딩 완료
            LoadNextSceneAsync(); // 리소스 로딩 완료 후 씬 로딩 시작
        });
    }

    private string LoadSceneResource()
    {
        switch(Main.NextScene)
        {
            case "TitleScene":
                return "TitleScene";
            case "EndingScene":
                return "EndingScene";
            default:
                return "GameScene";
        }
    }

    private void LoadNextSceneAsync()
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(Main.NextScene);
        if(Main.FirstLord)sceneLoadOperation.allowSceneActivation = false;
        StartCoroutine(UpdateLoadingProgress(sceneLoadOperation));
    }
    
    private IEnumerator UpdateLoadingProgress(AsyncOperation sceneLoadOperation)
    {
        
        while (!_resourcesLoaded || sceneLoadOperation.progress < 0.9f)
        {
            if(Main.FirstLord)progressBar.fillAmount = sceneLoadOperation.progress*0.1f; // 실제 로딩
            yield return null;
        }
        if (Main.FirstLord)
        {
            // 페이크 로딩
            while (progressBar.fillAmount < 1f)
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.09f, 1f, timer);
                yield return null;
            }

            anyKeyDownTxt.SetActive(true);

            // 페이크 로딩이 끝나면 키 입력을 대기합니다.
            while (!Input.anyKeyDown)
            {
                Main.FirstLord = false;
                yield return null;
            }
        }
        

        // 키 입력이 들어오면 씬 전환이 가능하게 설정합니다.
        sceneLoadOperation.allowSceneActivation = true;
    }

    public override void Clear()
    {

    }
}


