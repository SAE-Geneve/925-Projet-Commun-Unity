using UnityEngine;

public class ObjectPulsing : MonoBehaviour
{
    public float pulseSpeed = 5.0f;
    
    private Renderer _renderer;
    private Color _initialColor;
    private Prop _prop;

    private void Start()
    {
        _prop = GetComponent<Prop>();
        _renderer = GetComponentInChildren<Renderer>();
        
        if (_renderer != null)
        {
            _initialColor = _renderer.material.color;
        }
    }

    private void Update()
    {
        if (_renderer == null) return;

        if (_prop != null && _prop.Controller != null)
        {
            _renderer.material.color = _initialColor;
            return;
        }

        float t = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
        
        Color targetColor = _initialColor * 0.3f; 
        targetColor.a = _initialColor.a;
        
        _renderer.material.color = Color.Lerp(_initialColor, targetColor, t);
    }
}
