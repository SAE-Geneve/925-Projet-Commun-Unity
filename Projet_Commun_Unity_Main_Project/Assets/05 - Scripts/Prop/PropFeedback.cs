using System.Collections;
using UnityEngine;

public class PropFeedback : MonoBehaviour
{
    [Header("Settings")]
    public GameObject impactEffectPrefab;
    public float velocityThreshold = 2f;

    private Vector3 initialScale;
    private Coroutine wobbleCoroutine;

    private void Awake()
    {
        // On sauvegarde la scale d'origine dès le réveil de l'objet
        initialScale = transform.localScale;
    }

    public void PlayGrabEffect()
    {
        // 1. On lance les particules (ton effet actuel)
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. On lance l'animation de Scale (le rebond)
        if (wobbleCoroutine != null) StopCoroutine(wobbleCoroutine);
        wobbleCoroutine = StartCoroutine(GrabWobbleRoutine());
    }

    private IEnumerator GrabWobbleRoutine()
    {
        float elapsed = 0;
        
        Vector3 squashScale = new Vector3(initialScale.x * 1.2f, initialScale.y * 0.8f, initialScale.z * 1.2f);
        
        Vector3 stretchScale = new Vector3(initialScale.x * 0.8f, initialScale.y * 1.3f, initialScale.z * 0.8f);

        while (elapsed < 0.07f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, squashScale, elapsed / 0.07f);
            yield return null;
        }

        elapsed = 0;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squashScale, stretchScale, elapsed / 0.1f);
            yield return null;
        }

        elapsed = 0;
        while (elapsed < 0.15f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(stretchScale, initialScale, elapsed / 0.15f);
            yield return null;
        }

        transform.localScale = initialScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffectPrefab, contact.point, Quaternion.identity);
            
            if (wobbleCoroutine != null) StopCoroutine(wobbleCoroutine);
            wobbleCoroutine = StartCoroutine(GrabWobbleRoutine());
        }
    }
}