

using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DataManager 
{
    #region simpleData

    private GameData _data = new();
    public GameData Data => _data;


    public float MouseSense
    {
        get
        {
            if (_data.MouseSensitivity < 0)
            {
                if (PlayerPrefs.HasKey("MouseSensitivity"))
                    return _data.MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
                else
                {
                    PlayerPrefs.SetFloat("MouseSensitivity", 10.0f);
                    return _data.MouseSensitivity = 10.0f;
                }
            }
            return _data.MouseSensitivity;
        }
        set
        {
            PlayerPrefs.SetFloat("MouseSensitivity", value);
            _data.MouseSensitivity = value;
        }
    }

    public InputActionAsset Actions
    {
        get
        {
            if (_data.InputActions == null)
            {
                //TextAsset json = Resources.Load<TextAsset>("InputActions");
                //return _data.InputActions = InputActionAsset.FromJson(json.ToString());
                return _data.InputActions = Resources.Load<InputActionAsset>("PlayerController");
            }
            return _data.InputActions;
        }
    }
    #endregion

    #region Json
    public bool ContinueGame;
    /// <summary>
    /// 해당 json파일이 있는지 확인하는 메서드
    /// </summary>
    /// <param name="fileName"></param>
    public void CheckFile(string fileName)
    {
        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 데이터 폴더에 파일이 없으면 "Resources" 폴더에서 복사
        if (!File.Exists(persistentPath))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(fileName); // "Resources" 폴더에서 파일 읽기
            File.WriteAllText(persistentPath, textAsset.text); // 데이터 폴더에 파일 쓰기
        }
    }

    /// <summary>
    /// json파일을 읽어오는 메서드
    /// </summary>
    /// <returns></returns>
    public StageClear Lord()

    {
        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 데이터 폴더의 파일에서 JSON 문자열 읽기
        string json = File.ReadAllText(persistentPath);
        // JSON 문자열을 객체로 변환
        StageClear stageClear = JsonConvert.DeserializeObject<StageClear>(json);
        return stageClear;
    }

    /// <summary>
    /// 한번에 모든 데이터를 바꾸는 메서드
    /// </summary>
    /// <param name="stageDate"></param>
    public void ModifyWrite(StageClear stageDate)
    {

        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 객체를 다시 JSON 문자열로 변환
        string newJson = JsonConvert.SerializeObject(stageDate);

        // JSON 문자열을 데이터 폴더의 파일에 저장
        File.WriteAllText(persistentPath, newJson);
    }
    /// <summary>
    /// 스테이지 클리어값을 바꾸는 메서드
    /// </summary>
    /// <param name="stageClear"></param>
    public void ModifyStageClear(int stageClear)
    {
        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 파일에서 JSON 문자열 읽기
        string json = File.ReadAllText(persistentPath);

        // JSON 문자열을 객체로 변환
        StageClear stageDate = JsonConvert.DeserializeObject<StageClear>(json);

        // 속성 수정
        stageDate.stageClear = stageClear;

        // 객체를 다시 JSON 문자열로 변환
        string newJson = JsonConvert.SerializeObject(stageDate);

        // JSON 문자열을 데이터 폴더의 파일에 저장
        File.WriteAllText(persistentPath, newJson);
    }

    /// <summary>
    /// 현재 플레이중인 스테이지 이름을 바꾸는 메서드
    /// </summary>
    /// <param name="PlayStage"></param>
    public void ModifyPlayStage(string PlayStage)
    {
        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 파일에서 JSON 문자열 읽기
        string json = File.ReadAllText(persistentPath);

        // JSON 문자열을 객체로 변환
        StageClear stageDate = JsonConvert.DeserializeObject<StageClear>(json);

        // 속성 수정
        stageDate.playStage = PlayStage;

        // 객체를 다시 JSON 문자열로 변환
        string newJson = JsonConvert.SerializeObject(stageDate);

        // JSON 문자열을 데이터 폴더의 파일에 저장
        File.WriteAllText(persistentPath, newJson);
    }
    /// <summary>
    /// 마지막 체크포인트 위치를 저장하는 메서드
    /// </summary>
    /// <param name="vector3"></param>
    public void ModifyLastCheckPoint(Vector3 vector3)
    {
        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 파일에서 JSON 문자열 읽기
        string json = File.ReadAllText(persistentPath);

        // JSON 문자열을 객체로 변환
        StageClear stageDate = JsonConvert.DeserializeObject<StageClear>(json);

        // 속성 수정
        stageDate.lastCheckPoint = new PlayerVector3 { x = vector3.x, y = vector3.y, z = vector3.z };

        // 객체를 다시 JSON 문자열로 변환
        string newJson = JsonConvert.SerializeObject(stageDate);

        // JSON 문자열을 데이터 폴더의 파일에 저장
        File.WriteAllText(persistentPath, newJson);
    }

    /// <summary>
    /// 마지막에 있던 위치를 저장하는 메서드
    /// </summary>
    /// <param name="vector3"></param>
    public void ModifyNowPlayerPoint(Vector3 vector3)
    {
        string fileName = "PlayerDate";

        CheckFile(fileName);

        string persistentPath = Path.Combine(Application.persistentDataPath, fileName); // 데이터 폴더의 파일 경로

        // 파일에서 JSON 문자열 읽기
        string json = File.ReadAllText(persistentPath);

        // JSON 문자열을 객체로 변환
        StageClear stageDate = JsonConvert.DeserializeObject<StageClear>(json);

        // 속성 수정
        stageDate.nowPlayerPoint = new PlayerVector3 { x = vector3.x, y = vector3.y, z = vector3.z };

        // 객체를 다시 JSON 문자열로 변환
        string newJson = JsonConvert.SerializeObject(stageDate);

        // JSON 문자열을 데이터 폴더의 파일에 저장
        File.WriteAllText(persistentPath, newJson);
    }
    public class StageClear
    {
        //현재 클리어한 스테이지 수
        public int stageClear;
        //이어하기시 플레이중인 스테이지 이름
        public string playStage;
        //이어하기시 마지막에 저장된 체크포인트
        public PlayerVector3 lastCheckPoint;
        //이어하기시 플레이어가 최종으로 있던 위치
        public PlayerVector3 nowPlayerPoint;
        //이어할 데이터가 있는지 확인 초기값 flase
        public bool isPlaying;

        //lastCheckPoint값을 벡터로 바꿔 가져오는 메서드
        public Vector3 LastCheckPointToVector3()
        {
            return new Vector3(lastCheckPoint.x, lastCheckPoint.y, lastCheckPoint.z);
        }
        //nowPlayerPoint값을 벡터로 바꿔 가져오는 메서드
        public Vector3 NowPlayerPointToVector3()
        {
            return new Vector3(nowPlayerPoint.x, nowPlayerPoint.y, nowPlayerPoint.z);
        }
        //lastCheckPoint값을 벡터로 바꿔 저장하는 메서드
        public void LastCheckPointToVector3Save(Vector3 vector3)
        {
            lastCheckPoint.x = vector3.x;
            lastCheckPoint.y = vector3.y;
            lastCheckPoint.z = vector3.z;
        }
        //nowPlayerPoint값을 벡터로 바꿔 저장하는 메서드
        public void NowPlayerPointToVector3Save(Vector3 vector3)
        {
            nowPlayerPoint.x = vector3.x;
            nowPlayerPoint.y = vector3.y;
            nowPlayerPoint.z = vector3.z;
        }
    }

    public class PlayerVector3
    {
        public float x;
        public float y;
        public float z;
    }
    public void PlayerPlaySceneLord()
    {
        ContinueGame = true;
        Main.Scenes.LoadScene(Main.StageClear.playStage);
        //Main.Game.Player.transform.position = Main.StageClear.NowPlayerPointToVector3();
        //Main.Game.Player.GetComponent<PlayerCheckPoint>().LastCheckPoint = Main.StageClear.LastCheckPointToVector3();
    }
    public void PlayerPlaySceneSave()
    {
        StageClear stage = Main.StageClear;
        GameObject player = Main.Game.Player;

        stage.isPlaying = true;
        stage.LastCheckPointToVector3Save(player.GetComponent<PlayerCheckPoint>().LastCheckPoint);
        stage.NowPlayerPointToVector3Save(player.gameObject.transform.position);
        stage.playStage = SceneManager.GetActiveScene().name;
        ModifyWrite(stage);
    }
    public void PlayerLordSceneSetting()
    {
        if (Main.Data.ContinueGame)
        {
            CharacterController characterController = Main.Game.Player.GetComponent<CharacterController>();
            Debug.Log(Main.Game.Player);
            characterController.enabled = false;
            Main.Game.Player.transform.position = Main.StageClear.NowPlayerPointToVector3();
            characterController.enabled = true;
            
            Main.Game.Player.GetComponent<PlayerCheckPoint>().LastCheckPoint = Main.StageClear.LastCheckPointToVector3();

            Main.Data.ContinueGame = false;
        }
    }
    #endregion

    #region CSV
    public TextAsset CSVLoader(Define.CSV csv)
    {
        string name = Enum.GetName(typeof(Define.CSV), csv);
        return Resources.Load<TextAsset>(name);
    }

    /// <summary>
    /// CSV파일을 첫번째 열을 키값으로 하여 행단위로 나누는 메서드
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string[]> CSVWidthCutting(TextAsset csvText)
    {

        // 줄바꿈('\n')을 기준으로 CSV 데이터를 행(row)으로 분리합니다.
        string[] data = csvText.text.Split(new char[] { '\n' });
        Array.Resize(ref data, data.Length - 1);

        // 첫 번째 열을 키로, 나머지 열을 값으로 가지는 딕셔너리를 생성합니다.
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
        // 각 행을 처리합니다.
        for (int i = 0; i < data.Length; i++)
        {
            // 쉼표(',')를 기준으로 각 행을 열(column)로 분리합니다.
            string[] row = data[i].Split(new char[] { ',' });

            // 첫 번째 열을 제외한 나머지 열을 저장할 배열을 생성합니다.
            string[] values = new string[row.Length - 1];

            // 나머지 열들을 배열에 저장합니다.
            for (int j = 1; j < row.Length; j++)
            {
                if (row[j] != null) values[j - 1] = row[j];
                //null대신 ""인 빈값을 사용하고 싶을 때 위아래 바꾸면 됩니다.
                //values[j - 1] = row[j] ?? "";
            }

            // 첫 번째 열(row[0])을 키로, 나머지 열들(values)을 값으로 딕셔너리에 저장합니다.
            dict[row[0]] = values;
        }
        return dict;
    }

    /// <summary>
    /// CSV파일을 첫번째 행을 키값으로 하여 열단위로 나누는 메서드
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string[]> CSVLengthCutting(TextAsset csvText)
    {

        // 줄바꿈('\n')을 기준으로 CSV 데이터를 행(row)으로 분리합니다.
        string[] data = csvText.text.Split(new char[] { '\n' });
        Array.Resize(ref data, data.Length - 1);

        // 첫 번째 행을 키로 사용합니다.
        string[] keys = data[0].Split(new char[] { ',' });

        // 각 키에 대한 값들을 저장할 딕셔너리를 생성합니다.
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

        for (int j = 0; j < keys.Length; j++)
        {
            // 각 키에 대한 값을 저장할 배열을 생성합니다.
            dict[keys[j]] = new string[data.Length - 1];
            //Debug.Log(keys.Length);

        }

        // 두 번째 행부터 데이터를 처리합니다.
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            for (int j = 0; j < keys.Length && j < row.Length; j++)
            {
                // 첫 번째 행의 각 열을 키로, 현재 행의 같은 열을 값으로 저장합니다.
                if (row[j] != null) dict[keys[j]][i - 1] = row[j];
                //null대신 ""인 빈값을 사용하고 싶을 때 위아래 바꾸면 됩니다.
                //dict[keys[j]][i - 1] = row[j] ?? "";
            }
        }

        return dict;
    }

    /// <summary>
    /// 딕셔너리에서 키값으로 배열 가져오는 메서드
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string[] FindValuesByKey(Dictionary<string, string[]> dict, Enum key)
    {
        // Enum을 문자열로 변환합니다.
        string keyString = key.ToString();
        // 딕셔너리에서 키를 사용하여 값을 찾습니다.
        string[] values;
        if (dict.TryGetValue(keyString, out values))
        {
            // 값이 찾아졌다면, 그 값을 반환합니다.
            return values;
        }
        else
        {
            // 키에 해당하는 값이 딕셔너리에 없다면, null을 반환합니다.
            return null;
        }
    }

    /// <summary>
    /// 배열을 특정 키워드에 따라 잘라 키워드는 키값 나머지는 벨류값으로 나누기
    /// </summary>
    /// <param name="strings"></param>
    /// <param name="enumName"></param>
    public Dictionary<string, string[]> CSVSortationCutting(string[] strings, Type enumName)
    {

        // 결과를 저장할 딕셔너리를 생성합니다. 이 딕셔너리는 문자열을 키로, 문자열 배열을 값으로 가집니다.
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

        // 문자열을 임시로 저장할 리스트를 생성합니다.
        List<string> temp = new();
        string currentKey = null;

        // 원본 배열의 모든 원소에 대해 반복합니다.
        foreach (string s in strings)
        {
            // 현재 원소가 Enum 값 중 하나인지 판별합니다.
            if (!Enum.IsDefined(enumName, s))
            {
                // Enum 값이 아니라면 임시 리스트에 추가합니다.
                temp.Add(s);
            }
            else
            {
                // Enum 값이라면, 임시 리스트에 저장된 원소들을 새 배열로 만들어 딕셔너리에 추가합니다.
                if (temp.Count > 0 && currentKey != null)
                {
                    dict[currentKey] = temp.ToArray();
                    temp.Clear();
                }
                // 현재 원소를 새로운 키로 설정합니다.
                currentKey = s;
            }
        }

        // 마지막으로 임시 리스트에 남아 있는 원소들을 새 배열로 만들어 딕셔너리에 추가합니다.
        if (temp.Count > 0 && currentKey != null)
        {
            dict[currentKey] = temp.ToArray();
        }
        return dict;
    }
    #endregion
}
