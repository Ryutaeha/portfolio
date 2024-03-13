using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(Define.Scene type)
    {
        Debug.Log(CurrentScene);
        Debug.Log(type);
        CurrentScene.Clear();
        Main.NextScene = GetSceneName(type);
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log(CurrentScene);

        CurrentScene.Clear();
        Main.NextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public string GetSceneName(Define.Scene type)
    {
        string name = Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
    public void SetResolution()
    {

        int idx = PlayerPrefs.GetInt("Resolution");
        bool isFull = false;
        if (PlayerPrefs.GetInt("FullScreen") == 0)
            isFull = false;
        else
            isFull = true;

        int w = 1920, h = 1080;
        switch (idx)
        {
            case 0:
                w = 1920;
                h = 1080;
                break;
            case 1:
                w = 1280;
                h = 720;
                break;
            case 2:
                w = 1366;
                h = 768;
                break;
            case 3:
                w = 1536;
                h = 864;
                break;
            case 4:
                w = 2560;
                h = 1440;
                break;
            default:
                break;
        }

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장
        Screen.SetResolution(w, h, isFull);
        //Screen.SetResolution(w, (int)(((float)deviceHeight / deviceWidth) * w), isFull);

        //if ((float)w / h < (float)deviceWidth / deviceHeight)
        //{
        //    float newWidth = ((float)w / h) / ((float)deviceWidth / deviceHeight);
        //    if(Camera.main != null)
        //        Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        //}
        //else
        //{
        //    float newHeight = ((float)deviceWidth / deviceHeight) / ((float)w / h);
        //    if (Camera.main != null)
        //        Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        //}

    }
}
