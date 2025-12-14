using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior;

public class AIBorder : AIController
{
    private Transform conveyor;
    private Transform detector;
    private Transform throwHere;
    private Transform exitPoint;
    private PropManager propManager;

    public void Initialize(Transform conv, Transform det, Transform thr, Transform exit, PropManager propM)
    {
        conveyor = conv;
        detector = det;
        throwHere = thr;
        exitPoint = exit;
        propManager = propM;
        ApplyBlackboardValues();
    }
    
    private void ApplyBlackboardValues()
    {
        if (_behaviorAgent == null) return;

        if (conveyor != null) _behaviorAgent.SetVariableValue("ConveyorBelt", conveyor);
        if (detector != null) _behaviorAgent.SetVariableValue("Detector", detector);
        if (throwHere != null) _behaviorAgent.SetVariableValue("ThrowHere", throwHere);
        if (exitPoint != null) _behaviorAgent.SetVariableValue("Exit", exitPoint);
        if (propManager != null) _behaviorAgent.SetVariableValue("PropManager", propManager);
    }
}