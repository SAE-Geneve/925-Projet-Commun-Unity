using System.Collections;
using UnityEngine;

public class PropFeedback : MonoBehaviour
{
    [Header("Settings")]
    public GameObject impactEffectPrefab;
    public float velocityThreshold = 2f;
    [SerializeField] private AudioClip grabSound;

    private Vector3 initialScale;
    private Coroutine wobbleCoroutine;
    private AudioSource audioSource;

    private void Awake()
    {
        initialScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void PlayGrabEffect()
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Joue le son de grab
        if (grabSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(grabSound);
        }

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