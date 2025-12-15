using UnityEngine;

public class AIConveyorBelt : AIController
{
    private Transform exitPoint;
    private GameObject location;

    public void Initialize(GameObject loc, Transform exit)
    {
        exitPoint = exit;
        location = loc;
        ApplyBlackboardValues();
    }
    private void ApplyBlackboardValues()
    {

    }
}
