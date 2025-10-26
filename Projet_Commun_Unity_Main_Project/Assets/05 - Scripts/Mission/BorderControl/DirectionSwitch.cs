using System;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSwitch : ConveyorBelt
{
    [Header("Direction Switch")] 
    [SerializeField] private List<MultipleDirection> _multipleDirections;
    [SerializeField] private float _switchDelay = 3f;
    
    [Serializable]
    private struct MultipleDirection
    {
        public List<Vector2> Directions;
    }

    private List<Vector2> _directions;
    
    private int _multipleDirectionIndex;
    private int _directionIndex;

    private void Start()
    {
        _directions = _multipleDirections[_multipleDirectionIndex].Directions;
        
        InvokeRepeating(nameof(SwitchDirection), _switchDelay, _switchDelay);
    }

    private void SwitchDirection()
    {
        if (_directions.Count == 0)
        {
            Debug.LogWarning("directions are empty");
            return;
        }

        Vector2 direction2D = _directions[_directionIndex++ % _directions.Count];
        _direction = new Vector3(direction2D.x, 0, direction2D.y);
    }
}