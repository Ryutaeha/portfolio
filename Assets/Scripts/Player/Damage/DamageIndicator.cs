using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    #region fields
    public Image image;
    public float flashSpeed;
    private Coroutine _coFadeAwayCoroutine;
    #endregion

    private void Start()
    {
        //플레이어의 데미지입는 event에 등록
        Main.Game.Player.GetComponent<PlayerEventController>().OnTakeDamage += Flash;
    }

    /// <summary>
    /// 반투명의 빨간 이미지를 띄운 후 점점 사라지게 하는 메서드입니다.
    /// </summary>
    public void Flash()
    {
        if (_coFadeAwayCoroutine != null)
        {
            StopCoroutine(_coFadeAwayCoroutine);
        }

        image.enabled = true;
        image.color = Color.red;
        _coFadeAwayCoroutine = StartCoroutine(CoFadeAway());
    }

    private IEnumerator CoFadeAway()
    {
        float startAlpha = 0.5f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.unscaledDeltaTime;
            image.color = new Color(1.0f, 0.0f, 0.0f, a);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        image.enabled = false;
    }
}
