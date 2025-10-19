using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cam;
    [SerializeField] private PlayerManager playerManager;
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
            targetPosition.position = GetMeanVector();
        }
    }

    private Vector3 GetMeanVector(){
        
        float x = 0f;
        float y = 0f;
        float z = 0f;
 
        foreach (var pl in playerManager.Players)
        {
            x += pl.transform.position.x;
            y += pl.transform.position.y;
            z += pl.transform.position.z;
        }
        return new Vector3(x / playerManager.Players.Count, y / playerManager.Players.Count, z / playerManager.Players.Count);
    }
}
