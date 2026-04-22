using UnityEngine;

public class PropShop : Shop
{
    [Header("Prop References")] 
    [SerializeField] private Prop propPrefab;
    [SerializeField] private Transform spawnTransform;
    
    protected override bool Buy(PlayerController playerController)
    {
        Instantiate(propPrefab, spawnTransform.position, Quaternion.identity);
        return true;
    }
}
