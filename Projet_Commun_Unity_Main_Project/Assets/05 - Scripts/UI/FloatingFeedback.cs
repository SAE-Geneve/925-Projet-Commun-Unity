using UnityEngine;
using TMPro;

public class FloatingFeedback : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text feedbackText; // Accepte TextMeshPro ou TextMeshProUGUI

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifetime = 1.5f;

    private Color _originalColor;
    private float _timer;

    private void Start()
    {
        if (feedbackText != null)
        {
            _originalColor = feedbackText.color;
        }
    }

    private void Update()
    {
        // Fait monter le texte vers le haut
        transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
        
        _timer += Time.deltaTime;

        // Gère le fondu (Fade out)
        if (feedbackText != null)
        {
            float alpha = Mathf.Lerp(_originalColor.a, 0f, _timer / lifetime);
            feedbackText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
        }

        // Détruit l'objet à la fin du temps imparti
        if (_timer >= lifetime) Destroy(gameObject);
    }

    // Méthode appelée depuis PuddleTask pour définir le texte
    public void Setup(string text)
    {
        if (feedbackText != null) feedbackText.SetText(text);
    }
}