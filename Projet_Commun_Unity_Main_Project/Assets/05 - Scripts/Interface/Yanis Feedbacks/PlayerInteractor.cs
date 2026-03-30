using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private InteractionPromptUI promptUI;

    private IInteractable currentInteractable;
    private IInteractable previousInteractable;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        if (promptUI == null)
            Debug.LogError("PlayerInteractor: PromptUI non assigné.");
    }

    private void Update()
    {
        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact(playerController);
            Debug.Log("Interaction déclenchée.");
            promptUI.Flash();
        }
    }

    private void DetectInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

        float closestDistance = Mathf.Infinity;
        IInteractable closestInteractable = null;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();
            if (interactable == null)
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        if (closestInteractable != previousInteractable)
        {
            previousInteractable?.AreaExit();
            closestInteractable?.AreaEnter();
        }

        previousInteractable = closestInteractable;
        currentInteractable = closestInteractable;

        if (currentInteractable != null)
            promptUI.Show(currentInteractable.GetPromptText());
        else
            promptUI.Hide();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}