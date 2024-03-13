using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BtnPress : MonoBehaviour
{
    private bool isPressed = false;
    private bool isSoundPlayed = false;
    private GameObject hitButton;
    private PlayerEventController playerEventController;
    private PlayerPressButton buttonPushed;
    private UnifiedPressButton unifiedPressButton;

    private void Awake()
    {
        hitButton = null;
        playerEventController = GetComponent<PlayerEventController>();
    }

    private void Update()
    {
        GameObject currentGround = playerEventController.CheckGround();

        if (currentGround?.tag == "Button")
        {
            if (!isPressed) // 버튼이 눌리지 않은 상태에서 눌린 경우에만 처리
            {
                isPressed = true;
                hitButton = currentGround;
                hitButton.TryGetComponent<PlayerPressButton>(out buttonPushed);
                hitButton.TryGetComponent<UnifiedPressButton>(out unifiedPressButton);
                PressButton();
            }
        }
        else
        {
            if (isPressed) // 버튼이 눌린 상태에서 눌리지 않은 경우에만 처리
            {
                isPressed = false;
                ReleaseButton();
            }
        }
    }

    private void PressButton()
    {
        // 버튼 누르기 액션
        buttonPushed?.PressButtonFromPlayer();
        unifiedPressButton?.PressButtonFromPlayer();
        if (!isSoundPlayed)
        {
            //Main.Sound.PlayClipInSFX("DoorOpen", buttonPushed.Door.transform.position);
            isSoundPlayed = true;
        }
    }

    private void ReleaseButton()
    {
        // 버튼 떼기 액션
        buttonPushed?.PressButtonEndFromPlayer();
        unifiedPressButton?.PressButtonEndFromPlayer();
        isSoundPlayed = false;
    }
}
