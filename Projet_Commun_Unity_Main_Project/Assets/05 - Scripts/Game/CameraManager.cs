using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cam;
    private float _baseFOV;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform targetPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
        _baseFOV = _cam.Lens.FieldOfView;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerManager.Players.Count == 0)
        {
            
        }
        else
        {
            targetPosition.position = GetMeanVector();
        }
    }

    private Vector3 GetMeanVector(){
        int playerCount = playerManager.Players.Count;
        
        float x = 0f;
        float y = 0f;
        float z = 0f;
 
        foreach (var pl in playerManager.Players)
        {
            Vector3 pos = pl.transform.position;
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / playerCount, y / playerCount, z / playerCount);
    }
}
