using System.Collections;
using UnityEngine;

public class PropFeedback : MonoBehaviour
{
    [Header("Settings")]
    public GameObject impactEffectPrefab;
    public float velocityThreshold = 2f;

    [Tooltip("Délai minimum entre deux wobbles de collision (évite le spam sur les tapis)")]
    [SerializeField] private float _wobbleCooldown = 0.5f;

    private Coroutine wobbleCoroutine;
    private float _lastWobbleTime = -999f;

    public void PlayGrabEffect()
    {
        if (impactEffectPrefab != null)
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

        if (wobbleCoroutine != null) StopCoroutine(wobbleCoroutine);
        wobbleCoroutine = StartCoroutine(GrabWobbleRoutine());
    }

    private IEnumerator GrabWobbleRoutine()
    {
        Vector3 currentScale = transform.localScale;

        Vector3 squashScale  = new Vector3(currentScale.x * 1.2f, currentScale.y * 0.8f, currentScale.z * 1.2f);
        Vector3 stretchScale = new Vector3(currentScale.x * 0.8f, currentScale.y * 1.3f, currentScale.z * 0.8f);

        float elapsed = 0;
        while (elapsed < 0.07f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(currentScale, squashScale, elapsed / 0.07f);
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
            transform.localScale = Vector3.Lerp(stretchScale, currentScale, elapsed / 0.15f);
            yield return null;
        }

        transform.localScale = currentScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude <= velocityThreshold) return;
        if (Time.time - _lastWobbleTime < _wobbleCooldown) return;

        _lastWobbleTime = Time.time;

        if (impactEffectPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffectPrefab, contact.point, Quaternion.identity);
        }

        if (wobbleCoroutine != null) StopCoroutine(wobbleCoroutine);
        wobbleCoroutine = StartCoroutine(GrabWobbleRoutine());
    }
}