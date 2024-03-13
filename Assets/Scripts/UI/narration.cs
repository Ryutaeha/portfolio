using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class Narration : MonoBehaviour
{
    private TextMeshProUGUI narrationText;
    private NarrationData NarrationData;
    private List<AudioClip> narrationClips;
    private List<string> narrationTexts;
    private AudioSource audioSource;
    private Coroutine currentNarrationCoroutine; // 현재 실행 중인 코루틴을 참조하기 위한 변수
    private AudioMixerGroup NarrationMixerGroup; //나중에 오디오 믹서 - 나레이션
    private int languageValue;

    private void Awake()
    {
        languageValue = PlayerPrefs.GetInt("Language");
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        NarrationData = new NarrationData();
        narrationTexts = NarrationData.GetNarreationTexts(languageValue);
        narrationText = GetComponent<TextMeshProUGUI>();
        narrationClips = Main.Sound.NarrationClip;
        print(narrationClips.Count);
        NarrationMixerGroup = Main.Sound.NarrationMixerGroup;
        Main.Game.OnChangeLanguage += HandleLaguageChange;
        Main.Game.OnNarrationPlaying += HandleNarrationPlaying;
    }
    private void Update()
    {
        if (audioSource != null)
        {
            // TimeScale이 0이면 음성 일시 중지
            if (Time.timeScale == 0 && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            // TimeScale이 다시 1로 되면 재생
            else if (Time.timeScale != 0 && !audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }
    }

    private void HandleLaguageChange(int value)
    {
          narrationTexts = NarrationData.GetNarreationTexts(value);
    }

    private void HandleNarrationPlaying(int index, int count)
    {
        // Narration 객체가 유효한지 확인
        if (this == null || gameObject == null)
        {
            return; // 객체가 이미 파괴되었다면, 여기서 처리를 중단합니다.
        }

        // 현재 실행 중인 코루틴을 중단
        if (currentNarrationCoroutine != null)
        {
            StopCoroutine(currentNarrationCoroutine);
        }

        // 새로운 코루틴 시작
        currentNarrationCoroutine = StartCoroutine(PlayCoroutineNarration(index, count));
    }

    private IEnumerator PlayCoroutineNarration(int index, int count)
    {
        yield return new WaitForSeconds(1f);

        for (int i = index; i < count; i++)
        {
            if (i < 0 || i >= narrationClips.Count || i >= narrationTexts.Count)
            {
                Debug.LogError("PlayNarration: 인덱스가 범위를 벗어났습니다. " + narrationClips.Count + ", " + narrationTexts.Count);
                continue;
            }

            AudioClip clip = narrationClips[i];
            string text = narrationTexts[i];

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.outputAudioMixerGroup = NarrationMixerGroup;
                audioSource.Play();
            }
            if (narrationText != null)
            {
                narrationText.text = text;
            }
            yield return new WaitForSeconds(clip.length);
            narrationText.text = "";
            yield return new WaitForSeconds(1f);
        }

        currentNarrationCoroutine = null; // 나레이션 재생이 완료되면 참조 초기화
    }
}

