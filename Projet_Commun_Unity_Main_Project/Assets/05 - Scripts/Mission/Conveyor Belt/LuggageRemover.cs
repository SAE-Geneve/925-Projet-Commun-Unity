using UnityEngine;
using System.Collections;

public class LuggageRemover : MonoBehaviour
{
    [Header("Destruction Settings")]
    [Tooltip("Durée de l'animation de disparition finale")]
    [SerializeField] private float destroyDuration = 0.5f;
    
    [Tooltip("Nombre de tours complets avant destruction (1 tour = 2 passages)")]
    [SerializeField] private int lapsBeforeDestroy = 1;

    [Header("Shrink Settings")]
    [Tooltip("Durée de l'animation de rétrécissement à chaque passage")]
    [SerializeField] private float stepShrinkDuration = 0.5f;
    
    [Tooltip("Pourcentage de taille perdu à chaque passage (0.2 = perd 20% de sa taille actuelle)")]
    [SerializeField] private float shrinkFactorPerPass = 0.2f;

    private int _maxCheckpoints;

    private void Start()
    {
        _maxCheckpoints = lapsBeforeDestroy * 2 + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        ConveyorProp prop = other.GetComponentInParent<ConveyorProp>();

        if (prop != null)
        {
            prop.Lap++;
            if (prop.Lap >= _maxCheckpoints)
            {
                prop.enabled = false; 
                StartCoroutine(AnimateAndDestroy(prop.gameObject));
            }
            else
            {
                StartCoroutine(AnimateShrinkStep(prop.transform));
            }
        }
    }
    private IEnumerator AnimateShrinkStep(Transform target)
    {
        Vector3 startScale = target.localScale;
        Vector3 targetScale = startScale * (1.0f - shrinkFactorPerPass);
        
        float timer = 0f;

        while (timer < stepShrinkDuration)
        {
            if (target == null) yield break;

            timer += Time.deltaTime;
            float progress = timer / stepShrinkDuration;
            target.localScale = Vector3.Lerp(startScale, targetScale, progress);

            yield return null;
        }

        if (target != null)
        {
            target.localScale = targetScale;
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

        while (timer < destroyDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / destroyDuration;
            
            if (target != null)
            {
                target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            }
            else 
            { 
                yield break; 
            }
            
            yield return null;
        }
        
        if (target != null) Destroy(target);
    }
}