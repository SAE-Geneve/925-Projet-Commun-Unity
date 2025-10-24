using System.Collections.Generic;
using UnityEngine;

public class DirectionSwitch : ConveyorBelt
{
    private List<Vector2> directions = new();
    
    public void AddDirection(Vector2 dir) => directions.Add(dir);
}