using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckNextLocation", story: "[self] check if the next [location] is available in [locations]", category: "Action", id: "8cad99c8ce20d75b2bb61b273546f873")]
public partial class CheckNextLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Location;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Locations;
    [SerializeReference] public BlackboardVariable<bool> IsLast;
    
    // Nouvelle variable pour définir la distance d'arrêt (évite le jitter)
    [SerializeReference] public BlackboardVariable<float> StoppingDistance = new BlackboardVariable<float>(1.0f);

    protected override Status OnStart()
    {
        if (Self.Value == null || Locations.Value == null || Locations.Value.Count == 0)
        {
            return Status.Failure;
        }

        // ---------------------------------------------------------
        // CAS 1 : Entrée dans la file (Pas de location assignée)
        // ---------------------------------------------------------
        if (Location.Value == null)
        {
            GameObject firstObj = Locations.Value[0];
            if (firstObj == null) return Status.Failure;

            LocationPoint firstPoint = firstObj.GetComponent<LocationPoint>();

            if (firstPoint != null && firstPoint.available)
            {
                firstPoint.available = false;
                Location.Value = firstObj;
                IsLast.Value = (Locations.Value.Count == 1);
                return Status.Success; 
            }
            return Status.Failure; 
        }

        // ---------------------------------------------------------
        // CAS 2 : Déjà une location (Avancer ou Finir)
        // ---------------------------------------------------------
        int currentIndex = Locations.Value.IndexOf(Location.Value);
        if (currentIndex == -1) return Status.Failure; 

        // --> C'EST ICI QUE CA CHANGE <--
        // Si on est à la dernière position
        if (currentIndex >= Locations.Value.Count - 1)
        {
            IsLast.Value = true;

            // On vérifie la distance réelle entre l'IA et le point
            float distance = Vector3.Distance(Self.Value.transform.position, Location.Value.transform.position);

            // Si on est assez proche, on renvoie FAILURE pour arrêter de vouloir bouger
            if (distance <= StoppingDistance.Value)
            {
                return Status.Failure; // "J'ai fini, arrête de me faire bouger"
            }

            // Sinon, on renvoie SUCCESS pour continuer d'avancer vers le point
            return Status.Success;
        }

        IsLast.Value = false;

        // Logique classique pour avancer au point suivant
        int nextIndex = currentIndex + 1;
        GameObject nextObj = Locations.Value[nextIndex];
        if (nextObj == null) return Status.Failure;

        LocationPoint nextPoint = nextObj.GetComponent<LocationPoint>();

        if (nextPoint != null && nextPoint.available)
        {
            LocationPoint currentPoint = Location.Value.GetComponent<LocationPoint>();
            if (currentPoint != null) currentPoint.available = true;

            nextPoint.available = false;
            Location.Value = nextObj;

            return Status.Success; 
        }

        return Status.Failure;
    }
}