using UnityEngine;

public class AIBorder : AIController
{
    public void Initialize(Transform conv, Transform det, Transform thr, Transform exit, PropManager propM)
    {
        if (!_behaviorAgent) return;

        if (conv) _behaviorAgent.SetVariableValue("ConveyorBelt", conv);
        if (det) _behaviorAgent.SetVariableValue("Detector", det);
        if (thr) _behaviorAgent.SetVariableValue("ThrowHere", thr);
        if (exit) _behaviorAgent.SetVariableValue("Exit", exit);
        if (propM) _behaviorAgent.SetVariableValue("PropManager", propM);
    }
}