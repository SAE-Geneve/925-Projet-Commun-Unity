using UnityEngine;
using UnityEngine.UI;

public class GameStateButton : MonoBehaviour
{
    [SerializeField] private GameState _stateToEnable;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        
        GetComponent<Button>().onClick.AddListener(() => gameManager.SwitchState(_stateToEnable));   
    }
}