using UnityEngine;

public class TriggerScene : MonoBehaviour
{
    
    [SerializeField] string sceneToLoad;
    private int _playerNumber;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerNumber++;
        if(_playerNumber == PlayerManager.Instance.PlayerCount)
        {
            SceneLoader.Instance.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            _playerNumber--;
    }
}
