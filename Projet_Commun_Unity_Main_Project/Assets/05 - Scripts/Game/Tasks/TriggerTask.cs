using UnityEngine;

public class TriggerTask : GameTask
{
    [Header("Parameters")]
    [Tooltip("The object tag that needs to touch the task collider to succeed")]
    [SerializeField] string _taskTag;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(_taskTag) || Done) return;

        if (other.TryGetComponent(out Prop prop) && prop.IsGrabbed)
            prop.Dropped();
        
        Succeed();
        Destroy(other.gameObject);
        Done = true;
        
        Debug.Log($"Task {TaskName} done!");
    }
}
