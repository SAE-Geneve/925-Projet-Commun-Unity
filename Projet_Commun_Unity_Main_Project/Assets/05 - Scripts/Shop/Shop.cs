using TMPro;
using UnityEngine;

public abstract class Shop : MonoBehaviour, IInteractable
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI priceTmp;
    
    [Header("Parameters")] 
    [SerializeField] [Min(1)] private int price = 20;
    
    ObjectOutline _outline;
    
    private void Start()
    {
        _outline = GetComponent<ObjectOutline>();
        
        priceTmp.SetText($"{price}$");
    }

    protected abstract void Buy(PlayerController playerController);

    public void Interact(PlayerController playerController)
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;
        int playerId = playerController.Id;
        
        if (scoreManager.TotalScores[playerId] >= price)
        {
            Debug.Log("BUY");
            Buy(playerController);
            scoreManager.SubMissionScore(price, playerId);
        }
        else
        {
            Debug.Log("CAN'T BUY");
        }
    }

    public void InteractEnd() { }

    public void AreaEnter() => _outline.EnableOutline();

    public void AreaExit() => _outline.DisableOutline();
}