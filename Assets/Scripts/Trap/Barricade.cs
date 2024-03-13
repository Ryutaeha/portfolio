using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;
    [SerializeField] private bool ExitVer;
    private bool playerIn;
    private Vector3 beforePosition;


    public GameObject NewBarricade;

    private void Awake()
    {
        NewBarricade.SetActive(false);
        audioSource = NewBarricade.GetComponent<AudioSource>();
        audioClip = Main.Sound.FindAudioClip("DropSound");
        audioSource.clip = audioClip;
        beforePosition = NewBarricade.transform.position;            
    }

    private void Start()
    {
        Main.Game.Player.GetComponent<PlayerEventController>().OnDeath += UpBarricade;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!ExitVer)
            {
                BarricadeFall();
                this.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(ExitPlayer());
                playerIn = true;
            }
        }
    }

    private IEnumerator ExitPlayer()
    {
        if (playerIn)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(1.2f * Time.timeScale);
            BarricadeFall();
        }

    }
    private void BarricadeFall()
    {
        NewBarricade.SetActive(true);
        NewBarricade.transform.DOJump(new Vector3(NewBarricade.transform.position.x, NewBarricade.transform.position.y - 4f, NewBarricade.transform.position.z), 1f, 1, 0.2f).SetUpdate(UpdateType.Fixed, true);
        audioSource.Play();
    }

    private void UpBarricade()
    {
        NewBarricade.transform.DOMove(beforePosition, 1f);
        NewBarricade.SetActive(false);
        playerIn = false;
        this.gameObject.SetActive(true);
    }

}
