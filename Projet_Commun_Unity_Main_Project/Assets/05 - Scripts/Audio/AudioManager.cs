using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    //Maybe a bit stupid to put all the sound effects in the same script
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource continousSfxSource;
    
    [Header("Mini-Game Music")]
    [SerializeField] public AudioClip conveyorBeltMusic;
    [SerializeField] public AudioClip borderControleMusic;
    [SerializeField] public  AudioClip bordingMusic;
    [SerializeField] public AudioClip lostLuggageMusic;
    
    [Header("Other Musics")]
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip hubMusic;
    [SerializeField] public AudioClip endMusic;

    [Header("Sounds")]
    [SerializeField] public AudioClip successSFX;
    [SerializeField] public AudioClip failureSFX;
    [SerializeField] public AudioClip buttonSFX;
    [SerializeField] public AudioClip StartMissionSFX;
    [SerializeField] public AudioClip HitSFX;
    [SerializeField] public AudioClip StepSFX;
    [SerializeField] public AudioClip AnnonceEndMissionSFX;
    [SerializeField] public AudioClip ScanBorderSFX;
    [SerializeField] public AudioClip TapieRoulantSFX;
    [SerializeField] public AudioClip VoitureSFX;
    [SerializeField] public AudioClip LevierOnSFX;
    [SerializeField] public AudioClip LevierOffSFX;
    [SerializeField] public AudioClip Annonce1MinSFX;
    [SerializeField] public AudioClip Annonce30SecSFX;
    [SerializeField] public AudioClip TimeRemainingSFX;
    [SerializeField] public AudioClip CarpetBreakSFX;
    [SerializeField] public AudioClip WorsenCarpetBreakSFX;
    [SerializeField] public AudioClip RepairSFX;
    [SerializeField] public AudioClip FinishedRepairSFX;
    [SerializeField] public AudioClip ShopSFX;
    [SerializeField] public AudioClip EnterTriggerZoneSFX;
    [SerializeField] public AudioClip CountdownSFX;
    
    public static AudioManager Instance;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if(musicSource) musicSource.loop = true;
    }

    public void PlayBGM(AudioClip clip)
    {
        if(!musicSource) return;
        //Stops the previous background music
        musicSource.clip = clip;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }
    
    public void StopBGM()
    {
        //Stops the background music
        if(musicSource) musicSource.Stop();
    }

    public void PlaySfx(AudioClip clip)
    {
        //Allows for SFX to be played in other scripts
        if(sfxSource) sfxSource.PlayOneShot(clip);
    }
    public void PlayContinousSfx(AudioClip clip)
    {
        //Allows for SFX to be played in other scripts
        if(sfxSource) continousSfxSource.PlayOneShot(clip);
    }

    public void StopContinousSfx()
    {
        if(continousSfxSource) continousSfxSource.Stop();
    }

    public void SetVolumeMusic(float volume)
    {
        if (musicSource) musicSource.volume = volume;
    }
    
    public void SetVolumeSfx(float volume)
    {
        if (sfxSource) sfxSource.volume = volume;
    }

    public void SetSpeedMusic(float speed)
    {
        if (musicSource) musicSource.pitch = speed;
    }
    
    public void SetSpeedSfx(float speed)
    {
        if (sfxSource) sfxSource.pitch = speed;
    }
}
