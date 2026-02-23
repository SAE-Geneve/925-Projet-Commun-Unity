using UnityEngine;

public class DropZone : MonoBehaviour
{
    private bool isActiveTarget = false;

    public void SetActiveZone(bool status)
    {
        isActiveTarget = status;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActiveTarget) return;

        if (other.CompareTag("Valise"))
        {
            Debug.Log("La valise a été livrée dans la bonne zone !");
            
            LLGameManager.Instance.ValiseLivree();
        }
    }
}