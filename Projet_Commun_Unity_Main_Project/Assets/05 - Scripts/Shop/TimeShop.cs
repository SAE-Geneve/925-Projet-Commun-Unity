using UnityEngine;

public class TimeShop : Shop
{
    [Header("Timer Parameters")] 
    [SerializeField] [Min(0.1f)] private float additiveTime = 60f;
    
    protected override void Buy(PlayerController playerController)
    {
        GameManager gm = GameManager.Instance;
        
        if(gm) gm.Timer += additiveTime;
    }
}
