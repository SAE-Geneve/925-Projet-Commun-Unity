using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    
    [SerializeField] private Transform catchPoint;
    private bool isHolding = false;
    
    private Prop _grabbed;
    
    [Header("Throw Settings")]
    public float _maxThrowForce = 20f;
    public float _chargeSpeed = 10f;
    public float holdThreshold = 0.25f;
    private bool isPressing = false;
    private bool isCharging = false;
    private float pressStartTime = 0f;
    private float currentThrowForce = 0f;
    
    
    void Update()
    {
        if (isPressing && !isCharging)
        {
            if (Time.time - pressStartTime > holdThreshold)
            {
                isCharging = true;
                currentThrowForce = 0f;
            }
        }

        if (isCharging)
        {
            currentThrowForce += _chargeSpeed * Time.deltaTime;
            currentThrowForce = Mathf.Clamp(currentThrowForce, 0f, _maxThrowForce);
        }
    }

    public void CatchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
            if (!isHolding)
            {
                CatchBox();
                if(_grabbed != null)
                {
                    isHolding = true;
                    _grabbed.Grabbed();
                }
            }
            else
            {
                isPressing = true;
                isCharging = false;
                pressStartTime = Time.time;
                currentThrowForce = 0f;
            }
        }
        else if (context.canceled)
        {
            if (isPressing && isHolding)
            {
                float heldDuration = Time.time - pressStartTime;

                if (isCharging || heldDuration >= holdThreshold)
                {
                    ReleaseBoxWithForce(currentThrowForce);
                }
                else
                {
                    _grabbed.Dropped();
                    DropBox();
                }
                
                isPressing = false;
                isCharging = false;
                currentThrowForce = 0f;
                isHolding = false;
            }
            else
            {
                isPressing = false;
                isCharging = false;
                currentThrowForce = 0f;
            }
        }
    }
    
    void CatchBox()
    {
        var hits = Physics.SphereCastAll(transform.position, 1, Vector3.forward);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Prop grabbable))
            {
                _grabbed = grabbable;
                ParentConstraint pc = _grabbed.AddComponent<ParentConstraint>();
                pc.constraintActive = true;
                ConstraintSource source = new ConstraintSource();
                source.sourceTransform = catchPoint;
                source.weight = 1;
                pc.AddSource(source);
                
               _grabbed.GetComponent<Rigidbody>().isKinematic = true;
               var rb = _grabbed.GetComponent<Rigidbody>();
               if (rb != null) rb.isKinematic = true;
                return;
            }
        }
    }
    
    void ReleaseBox()
    {
        if (_grabbed != null)
        {
            ParentConstraint pc = _grabbed.GetComponent<ParentConstraint>();
            Destroy(pc);
            _grabbed.GetComponent<Rigidbody>().isKinematic = false;
            _grabbed = null;
        }
    }
    
    void DropBox()
    {
        if (_grabbed != null)
        {
            ParentConstraint pc = _grabbed.GetComponent<ParentConstraint>();
            if (pc != null) Destroy(pc);

            var rb = _grabbed.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            _grabbed = null;
        }
    }
    
    void ReleaseBoxWithForce(float force)
    {
        if (_grabbed != null)
        {
            ParentConstraint pc = _grabbed.GetComponent<ParentConstraint>();
            if (pc != null) Destroy(pc);

            var rb = _grabbed.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                
                Vector3 dir = transform.forward;
                rb.AddForce(dir.normalized * force, ForceMode.Impulse);
            }

            _grabbed = null;
        }
    }
}
