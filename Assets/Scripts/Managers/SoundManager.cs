using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public event EventHandler OnPlayerPickUp;
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioRefSO audioRefSO;


    private void Awake()
    {
        Instance = this;    
    }
   
    public void PlaySound(AudioClip[] audioClips , Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[UnityEngine.Random.Range(0,audioClips.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip,Vector3 position,float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
