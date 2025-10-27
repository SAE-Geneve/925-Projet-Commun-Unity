using UnityEngine;

public class StairKartCheck : MonoBehaviour
{
    
    bool _done = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StairKart"))
        {
            _done = true;
        }
    }    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StairKart"))
        {
            _done = false;
        }
    }
    
    public bool IsDone()
    {
        return _done;
    }
}
