using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.AreaEnter();
        else if (other.CompareTag("Prop") || other.CompareTag("Mop"))//Ajouter d'autres tags si besoin
        {
            Prop prop = other.GetComponentInParent<Prop>();
            if (prop != null) prop.AreaEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactable.AreaExit();
        else if (other.CompareTag("Prop") || other.CompareTag("Mop"))//Ajouter d'autres tag si besoin
        {
            Prop prop = other.GetComponentInParent<Prop>();
            if (prop != null) prop.AreaExit();
        }
    }
}
