using UnityEngine;
using Unity.Behavior; // Assurez-vous d'avoir le bon namespace

public class InitBorderNpc : MonoBehaviour
{
    private BehaviorGraphAgent _behaviorAgent; 

    private void Start()
    {
        _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (_behaviorAgent == null)
        {
            return;
        }
        
        
        GameObject conveyor = GameObject.Find("ConvoyorBelt");
        GameObject detector = GameObject.Find("ScanZone");
        GameObject throwHere = GameObject.Find("ThrowHere");
        GameObject exitPoint = GameObject.Find("ExitPoint");
        
        if (conveyor != null)
        {
            _behaviorAgent.SetVariableValue("ConveyorBelt", conveyor.transform);
        }
        else
        {
            Debug.LogError("Objet 'ConvoyorBelt' non trouvé dans la scène pour l'initialisation de l'IA.", this);
        }
        
        if (detector != null)
        {
            _behaviorAgent.SetVariableValue("Detector", detector.transform);
        }
        else
        {
            Debug.LogError("Objet 'ScanZone' non trouvé dans la scène pour l'initialisation de l'IA.", this);
        }

        // Répéter les vérifications pour throwHere et exitPoint...
        if (throwHere != null)
        {
            _behaviorAgent.SetVariableValue("ThrowHere", throwHere.transform);
        }
        
        if (exitPoint != null)
        {
            _behaviorAgent.SetVariableValue("Exit", exitPoint.transform);
        }
    }
}