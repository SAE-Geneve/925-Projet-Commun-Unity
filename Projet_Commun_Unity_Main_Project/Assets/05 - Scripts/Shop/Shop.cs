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
    
    public abstract void Interact(PlayerController playerController);

    public void InteractEnd() { }

    public void AreaEnter() => _outline.EnableOutline();

    public void AreaExit() => _outline.DisableOutline();
}