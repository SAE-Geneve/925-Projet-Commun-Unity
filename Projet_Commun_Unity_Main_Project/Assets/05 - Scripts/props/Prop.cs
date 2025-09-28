using UnityEngine;

public class Prop : MonoBehaviour, IGrabbable
{
    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void Grabbed()
    {
        Debug.Log("Grabbing");
    }

    public virtual void Dropped()
    {
        Debug.Log("Dropped");
    }
}
