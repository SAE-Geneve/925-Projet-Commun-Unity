using UnityEngine;
using UnityEngine.Serialization;

public class Boarding : MonoBehaviour
{
    [SerializeField] StairKartCheck kartPort1;
    [SerializeField] StairKartCheck kartPort2;
    [SerializeField] PropsSpawnerLimit propsSpawner;
    
    private bool _mission_done = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (propsSpawner.IsDone())
        {
            if (kartPort1.IsDone() || kartPort2.IsDone())
            {
                _mission_done = true;
            }
        }
    }
}
