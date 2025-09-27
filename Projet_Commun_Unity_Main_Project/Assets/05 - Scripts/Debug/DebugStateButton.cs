using UnityEngine;
using UnityEngine.UI;

public class DebugStateButton : MonoBehaviour
{
    [SerializeField] private GameState _stateToEnable;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        
        GetComponent<Button>().onClick.AddListener(() => gameManager.SwitchState(_stateToEnable));   
    }
}