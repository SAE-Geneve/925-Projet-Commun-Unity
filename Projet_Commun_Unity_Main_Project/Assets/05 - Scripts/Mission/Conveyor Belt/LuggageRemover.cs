using UnityEngine;
using System.Collections;

public class LuggageRemover : MonoBehaviour
{
    [Header("Destruction Settings")]
    [Tooltip("Durée de l'animation de disparition finale")]
    [SerializeField] private float destroyDuration = 0.5f;
    
    [Tooltip("Nombre de tours complets avant destruction (1 tour = 2 passages)")]
    public int lapsBeforeDestroy = 1;

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
        if (prop == null) return;

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

    private IEnumerator AnimateShrinkStep(Transform target)
    {
        Vector3 startScale = target.localScale;
        Vector3 targetScale = startScale * (1.0f - shrinkFactorPerPass);
        
        float timer = 0f;
        while (timer < stepShrinkDuration)
        {
            if (target == null) yield break;
            timer += Time.deltaTime;
            target.localScale = Vector3.Lerp(startScale, targetScale, timer / stepShrinkDuration);
            yield return null;
        }

        if (target != null)
            target.localScale = targetScale;
    }

    private IEnumerator AnimateAndDestroy(GameObject target)
    {
        if (target == null) yield break;

        Collider col = target.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        Vector3 originalScale = target.transform.localScale;
        float timer = 0f;

        while (timer < destroyDuration)
        {
            if (target == null) yield break;
            timer += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / destroyDuration);
            yield return null;
        }
        
        if (target != null) Destroy(target);
    }
    public int MaxCheckpoints => _maxCheckpoints;
}