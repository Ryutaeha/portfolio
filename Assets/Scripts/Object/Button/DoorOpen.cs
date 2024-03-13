using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject doorLeft;
    public GameObject doorRight;
    private AudioSource doorAudioSource;
    private AudioClip doorOpenSound;

    private bool doorOpen = false;
    private Vector3 leftDoorClosedPosition;

    private void Start()
    {
        doorAudioSource = GetComponent<AudioSource>();
        doorOpenSound = Main.Sound.FindAudioClip("DoorOpen");
        Debug.Log(doorOpenSound.name);
        doorAudioSource.clip = doorOpenSound;
    }

    public void OpenDoor()
    {
        if (doorOpen)
            return;

        doorOpen = true;
        leftDoorClosedPosition = doorLeft.transform.localPosition;
        StartCoroutine(PlaySound());

        Vector3 doorLeftOpenPosition = new Vector3(0, 0, -2);
        Vector3 doorRightOpenPosition = new Vector3(0, 0, 6);

        OpenCloseDoor(doorLeftOpenPosition, doorRightOpenPosition);
    }

    public void CloseDoor()
    {
        if (!doorOpen)
            return;

        doorOpen = false;

        Vector3 doorLeftClosedPosition = Vector3.zero;
        Vector3 doorRightClosedPosition = new Vector3(0, 0, 4);

        OpenCloseDoor(doorLeftClosedPosition, doorRightClosedPosition);
    }

    private void OpenCloseDoor(Vector3 leftTargetPosition, Vector3 rightTargetPosition)
    {
        Transform doorLeftTransform = doorLeft.transform;
        Transform doorRightTransform = doorRight.transform;

        DOTween.Sequence()
            .Append(doorLeftTransform.DOLocalMove(leftTargetPosition, 1f))
            .Join(doorRightTransform.DOLocalMove(rightTargetPosition, 1f))
            .Play();
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.3f);

        if (doorOpen)
        {
            if (leftDoorClosedPosition.z > -1)
            {
                Debug.Log("Play");
                doorAudioSource.Play();
            }
        }
    }
}