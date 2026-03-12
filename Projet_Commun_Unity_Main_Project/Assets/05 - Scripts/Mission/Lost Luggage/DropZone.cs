using UnityEngine;

public class DropZone : TriggerTask
{
    private bool isActiveTarget = false;

    [Header("Visuels")]
    public GameObject visualHighlight; 

    public void SetActiveZone(bool status)
    {
        isActiveTarget = status;
        if (visualHighlight != null) visualHighlight.SetActive(status);
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = status;
        
        if(status) ResetTask();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isActiveTarget) return;

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerValid(Prop prop)
    {
        if (!isActiveTarget) return;

        base.OnTriggerValid(prop);
    }
}