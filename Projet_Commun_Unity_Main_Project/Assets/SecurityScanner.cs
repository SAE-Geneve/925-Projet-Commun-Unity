using Unity.Behavior;
using UnityEngine;
public class SecurityScanner : MonoBehaviour
{
    
   
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
        if (other.gameObject.CompareTag("AI"))
        {
           //TODO: find a way to be able to access the BehaviorAgent Component
        }
    }
}
