using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float rotationSpeed = 10f;

    private void Update() => transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
}
