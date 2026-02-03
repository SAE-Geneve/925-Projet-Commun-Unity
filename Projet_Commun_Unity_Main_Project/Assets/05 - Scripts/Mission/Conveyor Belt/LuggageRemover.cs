using UnityEngine;
using System.Collections;

public class LuggageRemover : MonoBehaviour
{
    [Header("Settings")]
    
    [Tooltip("Temps que met l'objet à rétrécir avant de disparaitre")]
    [SerializeField] private float shrinkDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prop"))
        {
            StartCoroutine(AnimateAndDestroy(other.gameObject));
        }
    }

    private IEnumerator AnimateAndDestroy(GameObject target)
    {
        Collider col = target.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        Vector3 originalScale = target.transform.localScale;
        float timer = 0f;

        while (timer < shrinkDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / shrinkDuration;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            
            yield return null;
        }
        
        Destroy(target);
    }
}