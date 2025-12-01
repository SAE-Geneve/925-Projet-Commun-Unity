using UnityEngine;

public class DebugTriggerController : MonoBehaviour
{
    public bool Success { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Controller controller))
            Success = true;
    }
}
