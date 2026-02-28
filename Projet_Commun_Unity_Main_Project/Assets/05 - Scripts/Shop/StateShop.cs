using UnityEngine;

public class StateShop :Shop
{
    [Header("State Parameters")] 
    [SerializeField] private PlayerBonus PlayerBonus;
    
    public override void Interact(PlayerController playerController)
    {
        playerController.PlayerBonus += PlayerBonus;
    }
}
