using UnityEngine;

public class Prop : MonoBehaviour, IGrabbable
{
    public Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void Grabbed(Transform grabber)
    {
        //Debug.Log("Grabbing");
        //if (_rb != null) _rb.isKinematic = true;
    }

    public virtual void Dropped()
    {
        //Debug.Log("Dropped");
    }
}
