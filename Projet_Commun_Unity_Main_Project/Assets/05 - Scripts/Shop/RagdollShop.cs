using UnityEngine;

public class RagdollShop : Shop
{
    protected override void Buy(PlayerController playerController)
    {
        playerController.Ragdoll.IsImmune = true;
    }

    protected override bool BuyCondition(PlayerController playerController) => !playerController.Ragdoll.IsImmune;
}
