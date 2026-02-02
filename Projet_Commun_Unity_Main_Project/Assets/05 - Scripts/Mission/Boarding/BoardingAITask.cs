using UnityEngine;

public class BoardingAITask : GameTask
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AIController ai))
        {
            Succeed();
            
        }
    }
}