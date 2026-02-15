using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.AreaEnter();
        else if(other.CompareTag("Prop") && other.TryGetComponent(out Prop prop))
            prop.AreaEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable))
            interactable.AreaExit();
        else if(other.CompareTag("Prop") && other.TryGetComponent(out Prop prop))
            prop.AreaExit();
    }
}
