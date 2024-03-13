using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundEffects
{
    glassBreakSound,

}
public class SoundEffect : MonoBehaviour
{
    [SerializeField] private soundEffects soundEffects;
    public void PlaySFX()
    {
        Main.Sound.PlaySFX(soundEffects.ToString(), transform.position);//유리 깨지는 소리
    }
}
