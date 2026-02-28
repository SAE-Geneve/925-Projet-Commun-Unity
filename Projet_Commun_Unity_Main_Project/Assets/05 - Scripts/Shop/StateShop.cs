using UnityEngine;

public class StateShop :Shop
{
    [Header("State Parameters")] 
    [SerializeField] private PlayerBonus PlayerBonus;
    
    protected override void Buy(PlayerController playerController)
    {
        playerController.PlayerBonus += PlayerBonus;
    }
}
