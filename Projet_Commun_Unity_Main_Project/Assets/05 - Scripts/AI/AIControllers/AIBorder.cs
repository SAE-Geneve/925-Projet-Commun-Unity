using UnityEngine;

public class AIBorder : AIController
{
    public void Initialize(Transform conv, Transform det, Transform thr, Transform exit, PropManager propM)
    {
        if (!BehaviorAgent) return;

        if (conv) BehaviorAgent.SetVariableValue("ConveyorBelt", conv);
        if (det) BehaviorAgent.SetVariableValue("Detector", det);
        if (thr) BehaviorAgent.SetVariableValue("ThrowHere", thr);
        if (exit) BehaviorAgent.SetVariableValue("Exit", exit);
        if (propM) BehaviorAgent.SetVariableValue("PropManager", propM);
    }
}