using UnityEngine;

public class DropZone : MonoBehaviour
{
    private bool isActiveTarget = false;

    [Header("Visuels")]
    public GameObject visualHighlight; 

    public void SetActiveZone(bool status)
    {
        isActiveTarget = status;
        if (visualHighlight != null) visualHighlight.SetActive(status);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActiveTarget) return;

        Prop valiseProp = other.GetComponent<Prop>();

        if (valiseProp != null)
        {
            
            Debug.Log($"Valise livrée ! Le joueur {valiseProp.OwnerId} marque un point !");
            
            valiseProp.Destroy(); 
            
            LLGameManager.Instance.ValiseLivree();
        }
    }
}