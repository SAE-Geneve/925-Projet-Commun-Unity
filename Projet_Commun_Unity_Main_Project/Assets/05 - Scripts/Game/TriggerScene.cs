using UnityEngine;
using UnityEngine.Serialization;

public class TriggerScene : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string _sceneToLoad;
    
    
    
    private int _playerNumber;

    private void Start() => PlayerManager.Instance.OnPlayerRemoved += CheckPlayerNumber;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerNumber++;
        CheckPlayerNumber();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            _playerNumber--;
    }

    private void CheckPlayerNumber()
    {
        if(_playerNumber == PlayerManager.Instance.PlayerCount)
            SceneLoader.Instance.LoadScene(_sceneToLoad);
    }
}
