using UnityEngine;

public class StartMusicLobby : MonoBehaviour
{
    
    private AudioManager _audioManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
        _audioManager.PlayBGM(_audioManager.lobbyMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
