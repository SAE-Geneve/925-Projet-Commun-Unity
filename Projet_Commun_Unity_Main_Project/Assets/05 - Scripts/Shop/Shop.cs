using TMPro;
using UnityEngine;

public abstract class Shop : MonoBehaviour, IInteractable
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI priceTmp;
    
    [Header("Parameters")] 
    [SerializeField] [Min(1)] private int price = 20;
    
    ObjectOutline _outline;
    Animator _animator;
    
    private void Start()
    {
        _outline = GetComponent<ObjectOutline>();
        _animator = GetComponent<Animator>();
        
        priceTmp.SetText($"{price}$");
    }

    protected abstract void Buy(PlayerController playerController);

    public void Interact(PlayerController playerController)
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;
        int playerId = playerController.Id;
        
        if (scoreManager.TotalScores[playerId] >= price)
        {
            Buy(playerController);
            scoreManager.SubTotalScore(price, playerId);
            _animator.SetTrigger("Buy");
        }
        else _animator.SetTrigger("Fail");
    }

    public void InteractEnd() { }

    public void AreaEnter() => _outline.EnableOutline();

    public void AreaExit() => _outline.DisableOutline();
}