using UnityEngine;

public class TriggerTask : GameTask
{
    [Header("Parameters")]
    [SerializeField] private string _taskName = "New Task";
    [Tooltip("The object tag that needs to touch the task collider to succeed")]
    [SerializeField] string _taskTag;

    private bool _done;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(_taskTag) || _done) return;

        if (other.TryGetComponent(out Prop prop) && prop.IsGrabbed)
            prop.Dropped();
        
        Succeed();
        Destroy(other.gameObject);
        // _done = true;
        
        Debug.Log($"Task {_taskName} done!");
    }
}
