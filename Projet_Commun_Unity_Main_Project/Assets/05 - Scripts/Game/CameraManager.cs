using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cam;
    [SerializeField] private PlayerManager playerManager;
    private readonly List<Vector3> _positions = new List<Vector3>();
    [SerializeField] private Transform targetPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerManager.Players.Count == 0)
        {
            
        }
        else
        {
            _positions.Clear();
            foreach (var pl in playerManager.Players)
            {
                _positions.Add(pl.transform.position);
            }
            
            targetPosition.position = GetMeanVector();
        }
    }

    private Vector3 GetMeanVector(){
        if (_positions.Count == 0)
            return Vector3.zero;
         
        float x = 0f;
        float y = 0f;
        float z = 0f;
 
        foreach (Vector3 pos in _positions)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / _positions.Count, y / _positions.Count, z / _positions.Count);
    }
}
