using System;
using UnityEngine;

public class ClockHands : MonoBehaviour
{
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject secondHand;

    private DateTime startTime;
    private float elapsedTimeInSeconds;

    private void Start()
    {
        // 시작 시 현재 시간 저장
        startTime = DateTime.Now;
    }

    private void Update()
    {
        // 경과 시간 업데이트 (초 단위)
        elapsedTimeInSeconds += Time.deltaTime;

        // 시작 시간에 경과 시간을 더하여 현재 시간 계산
        DateTime currentTime = startTime.AddSeconds(elapsedTimeInSeconds);

        // 시, 분, 초 각도 계산
        float hourAngle = (currentTime.Hour % 12) * 30 + (currentTime.Minute * 0.5f);
        float minuteAngle = currentTime.Minute * 6 + (currentTime.Second * 0.1f);
        float secondAngle = currentTime.Second * 6;

        // 각도 적용
        ApplyRotation(hourHand, hourAngle);
        ApplyRotation(minuteHand, minuteAngle);
        ApplyRotation(secondHand, secondAngle);
    }

    private void ApplyRotation(GameObject hand, float angle)
    {
        if (hand != null)
            hand.transform.rotation = Quaternion.Euler(new Vector3(hand.transform.parent.eulerAngles.x, hand.transform.parent.eulerAngles.y, hand.transform.parent.eulerAngles.z + angle));
    }
}
