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
    
    // La variable que tu avais oubliée pour savoir si on est au bout de la file
    [SerializeReference] public BlackboardVariable<bool> IsLast;

    protected override Status OnStart()
    {
        if (Self.Value == null || Location.Value == null || Locations.Value == null)
        {
            return Status.Failure;
        }

        // 1. Trouver l'index de la location actuelle dans la liste
        int currentIndex = Locations.Value.IndexOf(Location.Value);

        // Si la location actuelle n'est pas dans la liste, c'est une erreur
        if (currentIndex == -1)
        {
            return Status.Failure;
        }

        // 2. Vérifier si on est à la dernière position (fin de la file indienne)
        if (currentIndex >= Locations.Value.Count - 1)
        {
            IsLast.Value = true;
            return Status.Success; // On est arrivé au bout
        }

        IsLast.Value = false;

        // 3. Regarder le point suivant
        int nextIndex = currentIndex + 1;
        GameObject nextObj = Locations.Value[nextIndex];
        
        if (nextObj == null) return Status.Failure;

        LocationPoint nextPoint = nextObj.GetComponent<LocationPoint>();
        
        // 4. Si le point suivant est disponible
        if (nextPoint != null && nextPoint.available)
        {
            // On libère le point actuel (celui qu'on quitte)
            LocationPoint currentPoint = Location.Value.GetComponent<LocationPoint>();
            if (currentPoint != null)
            {
                currentPoint.available = true;
            }

            // On réserve le point suivant
            nextPoint.available = false;

            // On met à jour la variable Location du Blackboard pour que le MoveTo aille au nouveau point
            Location.Value = nextObj;

            return Status.Success; // Succès : on a trouvé une place, on peut bouger
        }

        // 5. Si le point suivant est occupé, on échoue (l'IA attendra et réessaiera)
        return Status.Failure;
    }
}