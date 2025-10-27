using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Maybe a bit stupid to put all the sound effects in the same script
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Music")]
    [SerializeField] public AudioClip lobbyMusic;
    [SerializeField] public AudioClip minigameMusic;

    [Header("Sounds")]
    [SerializeField] public AudioClip successSFX;
    [SerializeField] public AudioClip failureSFX;
    [SerializeField] public AudioClip buttonSFX;
    
    public static AudioManager Instance;
    
    void Awake()
    {
        musicSource.loop = true;
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayBGM(AudioClip clip)
    {
        //Stops the previous background music
        musicSource.clip = clip;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        //Allows for SFX to be played in other scripts
        sfxSource.PlayOneShot(clip);
    }
}
