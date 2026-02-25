using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] protected Vector3 _direction = Vector3.forward;
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float materialSpeed = 1.5f;
    
    private readonly List<Prop> _props = new();
    
    private Renderer _renderer;
    private Vector2 _offset;
    
    private bool _isRunning = true;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(!_isRunning) return;
        
        float scrollDir = 0f;

        if (Mathf.Abs(_direction.z) > 0.01f) 
            scrollDir = Mathf.Sign(_direction.z);
        else if (Mathf.Abs(_direction.x) > 0.01f)
            scrollDir = _direction.x > 0 ? 1f : -1f;

        _offset += new Vector2(0f, scrollDir * materialSpeed * Time.deltaTime);

        Material[] mats = _renderer.materials;
        mats[1].mainTextureOffset = _offset;
        _renderer.materials = mats;
    }

    private void FixedUpdate()
    {
        if (!_isRunning) return;

        foreach (Prop prop in _props)
            prop.Rb.AddForce(_direction.normalized * _speed, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Prop" || !other.TryGetComponent(out ConveyorProp prop)) return;
        
        prop.AddConveyorBelt(this);
        _props.Add(prop);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Prop" || !other.TryGetComponent(out ConveyorProp prop)) return;
        
        prop.RemoveConveyorBelt(this);
        Remove(prop);
    }
    
    public void Remove(ConveyorProp prop) => _props.Remove(prop);
    
    public void StartBelt() => _isRunning = true;
    public void StopBelt() => _isRunning = false;
}