using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private PlayerEventController playerEventController = null;
    [SerializeField] private TMP_Text bindingDisplayNameTxt = null;
    [SerializeField] private GameObject startRebindingObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;
    [SerializeField] private GameObject player = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        player = GameObject.Find("Player(Clone)");
        if (player != null)
        {
            playerEventController = player.GetComponent<PlayerEventController>();
            if (playerEventController == null)
            {
                Debug.LogError("PlayerEventController component not found on the Player GameObject.");
            }
            else
            {
                Debug.Log(player.name);
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    public void StartRebinding()
    {
        Debug.Log("!11");
        startRebindingObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        //playerEventController.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        Debug.Log("1212");
        int bindingIndex = jumpAction.action.GetBindingIndexForControl(jumpAction.action.controls[0]);

        bindingDisplayNameTxt.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startRebindingObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        //playerEventController.PlayerInput.SwitchCurrentActionMap("Player");
    }
}
