using UnityEngine;

// Exemple minimal d'un IInteractable
public class TestInteractableCube : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log(gameObject.name + " a été interacté par ");

    }
}