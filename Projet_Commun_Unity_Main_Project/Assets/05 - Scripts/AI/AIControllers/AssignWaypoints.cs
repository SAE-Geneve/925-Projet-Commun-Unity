using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AssignWaypoints : MonoBehaviour
{
    [SerializeField] private LocationPoint[] wayPoints;
    [SerializeField]BehaviorGraphAgent agent;
    void Start()
    {

        if (agent)
        {
            List<GameObject> locationObjects = new List<GameObject>();
            foreach (var point in wayPoints)
            {
                if (point != null) 
                {
                    locationObjects.Add(point.gameObject);
                }
            }
            agent.SetVariableValue("Waypoints", locationObjects);
        }
    }
}
