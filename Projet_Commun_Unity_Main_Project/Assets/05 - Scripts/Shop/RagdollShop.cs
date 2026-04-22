using UnityEngine;

public class RagdollShop : Shop
{
    protected override bool Buy(PlayerController playerController)
    {
        playerController.Ragdoll.IsImmune = true;
        return true;
    }

    protected override bool BuyCondition(PlayerController playerController) => !playerController.Ragdoll.IsImmune;
}
