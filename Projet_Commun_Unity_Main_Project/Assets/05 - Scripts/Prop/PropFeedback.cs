using UnityEngine;

public class PropFeedback : MonoBehaviour
{
    [Header("Settings")]
    public GameObject impactEffectPrefab;
    public float velocityThreshold = 2f;

    public void PlayGrabEffect()
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffectPrefab, contact.point, Quaternion.identity);
        }
    }
}