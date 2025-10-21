using System;
using UnityEngine;

public class TEMPAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Music")]
    [SerializeField] public AudioClip conveyorbeltMusic;

    [Header("Sounds")]
    [SerializeField] public AudioClip successSFX;
    [SerializeField] public AudioClip failureSFX;

    private void Start()
    {
        musicSource.clip = conveyorbeltMusic;
        musicSource.Play();
    }

    public void PlayBGM(AudioClip clip)
    {
        //Stops the background music
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        //Allows for SFX to be played in other scripts
        sfxSource.PlayOneShot(clip);
    }
}
