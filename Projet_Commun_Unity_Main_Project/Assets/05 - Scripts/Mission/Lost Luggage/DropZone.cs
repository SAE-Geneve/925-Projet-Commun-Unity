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

        // On essaie de récupérer ton composant Prop sur l'objet qui entre
        Prop valiseProp = other.GetComponent<Prop>();

        // Si l'objet a bien le script Prop (donc c'est une valise ou un objet attrapable)
        if (valiseProp != null)
        {
            // Optionnel : Tu peux même vérifier si c'est bien une valise grâce à ton enum !
            // if (valiseProp.Type == PropType.RedLuggage || valiseProp.Type == PropType.BlueLuggage)
            
            Debug.Log($"Valise livrée ! Le joueur {valiseProp.OwnerId} marque un point !");
            
            // On utilise TA fonction pour la détruire, ce qui va forcer le joueur à la lâcher proprement !
            valiseProp.Destroy(); 
            
            // On prévient le GameManager de relancer la boucle
            LLGameManager.Instance.ValiseLivree();
        }
    }
}