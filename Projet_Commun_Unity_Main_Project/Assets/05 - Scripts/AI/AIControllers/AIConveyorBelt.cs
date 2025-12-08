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
        if (_behaviorAgent == null) return;
        if (exitPoint != null) _behaviorAgent.SetVariableValue("Exit", exitPoint);
        if (location != null) _behaviorAgent.SetVariableValue("Location", location);
    }
}
