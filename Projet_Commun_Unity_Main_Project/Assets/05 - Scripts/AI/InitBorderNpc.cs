using System.Linq;
using Unity.Behavior;
using UnityEngine;

public class InitBorderNpc : MonoBehaviour
{
    public BehaviorGraphAgent behaviorAgent;

    private void Awake()
    {

        if (behaviorAgent == null)
        {
            Debug.LogError("AIManager : BehaviorGraphAgent non assigné !");
            return;
        }
        
        GameObject conveyor = GameObject.Find("ConvoyorBelt");
        GameObject detector = GameObject.Find("ScanZone");
        GameObject throwHere = GameObject.Find("ThrowHere");
        GameObject exitPoint = GameObject.Find("ExitPoint");

        behaviorAgent.SetVariableValue("ConveyorBelt", conveyor.transform );
        behaviorAgent.SetVariableValue("Detector", detector.transform );
        behaviorAgent.SetVariableValue("ThrowHere", throwHere.transform );
        behaviorAgent.SetVariableValue("Exit", exitPoint.transform );
    }
}
