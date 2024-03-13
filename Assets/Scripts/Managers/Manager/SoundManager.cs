

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }
    [SerializeField] private Sound[] bgm;
    [SerializeField] private Sound[] sfx;
    [SerializeField] private AudioClip[] narration;

    //public Sound[] step = new Sound[2];
    private AudioSource bgmPlay { get; set; }
    private readonly List<AudioSource> _sfxPlays = new List<AudioSource>();
    //private AudioSource clockSound { get; set; }

    public AudioMixerGroup MasterMixerGroup;
    public AudioMixerGroup MusicMixerGroup;
    public AudioMixerGroup SFXMixerGroup;
    public AudioMixerGroup NarrationMixerGroup;
    public AudioMixer audioMixer;


    private GameData _data = new();
    public GameData Data => _data;

    public List<AudioClip> NarrationClip => narration?.ToList<AudioClip>();
   
    public float MasterVolume
    {
        get
        {
            if (_data.MasterVolume > 0)
            {
                if (PlayerPrefs.HasKey("MasterVolume"))
                    return _data.MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
                else
                {
                    PlayerPrefs.SetFloat("MasterVolume", 0f);
                    return _data.MasterVolume = 0f;
                }
            }
            return _data.MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            _data.MasterVolume = value;
        }
    }
    public float MusicVolume
    {
        get
        {
            if (_data.MusicVolume > 0)
            {
                if (PlayerPrefs.HasKey("MusicVolume"))
                    return _data.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
                else
                {
                    PlayerPrefs.SetFloat("MusicVolume", 0f);
                    return _data.MusicVolume = 0f;
                }
            }
            return _data.MusicVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            _data.MusicVolume = value;
        }
    }
    public float SFXVolume
    {
        get
        {
            if (_data.SFXVolume > 0)
            {
                if (PlayerPrefs.HasKey("SFXVolume"))
                    return _data.SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
                else
                {
                    PlayerPrefs.SetFloat("SFXVolume", 0f);
                    return _data.SFXVolume = 0f;
                }
            }
            return _data.SFXVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            _data.SFXVolume = value;
        }
    }
    public float NarrationVolume
    {
        get
        {
            if (_data.NarrationVolume > 0)
            {
                if (PlayerPrefs.HasKey("NarrationVolume"))
                    return _data.NarrationVolume = PlayerPrefs.GetFloat("NarrationVolume");
                else
                {
                    PlayerPrefs.SetFloat("NarrationVolume", 0f);
                    return _data.NarrationVolume = 0f;
                }
            }
            return _data.NarrationVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("NarrationVolume", value);
            _data.NarrationVolume = value;
        }
    }

    private void Start()
    {
        //Main.Sound = gameObject.GetComponent<SoundManager>();
        //씬 이동에 따라 이름 바꿀 수 있도록 만들어주세용

        SetVolume("Master", MasterVolume);
        SetVolume("Music", MusicVolume);
        SetVolume("SFX", SFXVolume);
        SetVolume("Narration", NarrationVolume);

        print(Main.Scenes.CurrentScene.SceneType);

        Define.Scene sceneType = Main.Scenes.CurrentScene.SceneType;
        switch (sceneType)
        {
            case Define.Scene.TitleScene:
                PlayBGM("MainSceneBGM");
                break;
            case Define.Scene.GameScene_Tutorial:
            case Define.Scene.GameScene_Tutorial2:
            case Define.Scene.GameScene_Tutorial3:
            case Define.Scene.GameScene_Tutorial4:
            case Define.Scene.GameScene_Stage1:
            case Define.Scene.GameScene_Stage2:
            case Define.Scene.GameScene_Stage3:
            case Define.Scene.GameScene_Stage4:
                PlayBGM("StartSceneBGM");
                break;
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (volume == -40f)
        {
            audioMixer.SetFloat(name, -80);
        }
        else
        {
            audioMixer.SetFloat(name, volume);
        }
    }

    public void InitializedSound()
    {
        GameObject soundManager = Main.Resource.Instantiate("SoundManager.prefab");
        soundManager.name = "@SoundManager";
        SoundManager sm = soundManager.GetComponent<SoundManager>();
        Main.Sound = sm;
        sm.LoadAllClip();
        Main.Sound.StartBGM();
    }

    public void StartBGM() //오디오소스 추가
    {
        bgmPlay = gameObject.GetComponent<AudioSource>();
        AudioSource sfxPlayer = gameObject.AddComponent<AudioSource>();
        _sfxPlays.Add(sfxPlayer);
    }

    // 배경음악 재생
    public void PlayBGM(string bgmName, float volume = 0.15f)
    {
        foreach (var t in bgm)
        {
            if (bgmName != t.name) continue;
            bgmPlay.clip = t.clip;
            bgmPlay.volume = volume;
            bgmPlay.outputAudioMixerGroup = MusicMixerGroup;
            bgmPlay.Play();
            return;
        }
    }

    public void StopBGM()
    {
        bgmPlay.Stop();
    }

    private void Update()
    {
        if (bgmPlay.pitch != Time.timeScale)
            bgmPlay.pitch = Time.timeScale;
    }

    // AudioClip을 역방향으로 만드는 메소드
    private AudioClip ReverseClip(AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        float[] reversedSamples = new float[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            reversedSamples[i] = samples[samples.Length - i - 1];
        }

        AudioClip reversedClip = AudioClip.Create(clip.name + "_Reversed", clip.samples, clip.channels, clip.frequency, false);
        reversedClip.SetData(reversedSamples, 0);

        return reversedClip;
    }

    // 역방향으로 사운드 효과를 재생하는 메소드
    public void PlaySFXReversed(string sfxName, Vector3 position, float volume = 0.3f)
    {
        foreach (var t in sfx)
        {
            if (sfxName != t.name)
            {
                continue;
            }
            AudioSource sfxPlay = AddSFXPlayer(); // AddSFXPlayer 메소드는 AudioSource를 추가하고 반환한다고 가정

            // AudioSource의 인스펙터 값을 조정
            sfxPlay.transform.position = position;
            sfxPlay.outputAudioMixerGroup = SFXMixerGroup; // 출력
            sfxPlay.spatialBlend = 1f; // 3D 사운드
            sfxPlay.maxDistance = 50f;
            sfxPlay.minDistance = 5f;
            sfxPlay.volume = volume;
            sfxPlay.playOnAwake = false;

            sfxPlay.clip = ReverseClip(t.clip); // 역방향 클립을 설정
            sfxPlay.Play();
            StartCoroutine(DestroyAfterPlay(sfxPlay)); // 재생 후 삭제

            return;
        }
    }


    // 되감을 수 있는 효과음 재생
    public void PlaySFX(string sfxName, Vector3 position, float volume = 0.3f)
    {
        foreach (var t in sfx)
        {
            if (sfxName != t.name)
            {
                continue;
            }
            AudioSource sfxPlay = AddSFXPlayer();

            //audioSource의 inspector 값 조절
            sfxPlay.transform.position = position;
            sfxPlay.outputAudioMixerGroup = SFXMixerGroup; //output
            sfxPlay.spatialBlend = 1f; //3D
            sfxPlay.maxDistance = 10f;
            sfxPlay.minDistance = 2f;
            sfxPlay.volume = volume;
            sfxPlay.playOnAwake = false;

            sfxPlay.clip = t.clip;
            sfxPlay.Play();
            StartCoroutine(DestroyAfterPlay(sfxPlay));

            return;
        }
    }


    public AudioClip FindAudioClip(string sfxName)
    {
        foreach (var t in sfx)
        {
            if (sfxName == t.name)
            {
                Debug.Log(t.name);
                return t.clip;
            }
        }
        Debug.Log(sfxName + " null");
        // 찾지 못한 경우에는 null을 반환합니다.
        return null;
    }

    private IEnumerator DestroyAfterPlay(AudioSource audioSource)
    {
        // 소리가 재생되는 동안 기다림
        yield return new WaitForSeconds(audioSource.clip.length);

        // 재생이 끝나면 해당 AudioSource를 제거
        _sfxPlays.Remove(audioSource);
        Destroy(audioSource);
    }

    private AudioSource AddSFXPlayer()
    {
        foreach (var t in _sfxPlays.Where(t => !t.isPlaying)) return t;

        // 모든 SFX 플레이어가 사용 중일 경우 새 플레이어 생성
        AudioSource newSFXPlay = gameObject.AddComponent<AudioSource>();
        _sfxPlays.Add(newSFXPlay);
        return newSFXPlay;
    }
    private void LoadAllClip()
    {
        foreach(var c in bgm)
        {
            c.clip.LoadAudioData();
        }
        foreach (var c in sfx)
        {
            c.clip.LoadAudioData();
        }
        //foreach (var c in step)
        //{
        //    c.clip.LoadAudioData();
        //}
    }
}
