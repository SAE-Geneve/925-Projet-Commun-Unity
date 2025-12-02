using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GrabTheNearestObject", story: "[self] grab an [object] and assign [GrabbedObject]", category: "Action", id: "ai_grabandthrow_random")]
public partial class GrabTheNearestObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<float> radius = new BlackboardVariable<float>(2);
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<string> Object;
    [SerializeReference] public BlackboardVariable<GameObject> GrabbedObject;
    
    private AIMovement aiMove; 
    private Controller _controller; 
    private Transform selfTransform;
    private IGrabbable targetGrabbable;
    
    private enum Phase { Searching, Moving, Done }
    private Phase phase;
    
    private bool searchFailed; 

    protected override Status OnStart()
    {
        if (Self == null || Self.Value == null)
        {
            return Status.Failure;
        }
        
        aiMove = Self.Value.GetComponent<AIMovement>();
        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        if (aiMove == null || _controller == null)
        {
            return Status.Failure;
        }

        phase = Phase.Searching;
        targetGrabbable = null;
        searchFailed = false;
        

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (searchFailed)
        {
            return Status.Failure;
        }
        
        if (phase == Phase.Done)
        {
            return Status.Success;
        }

        switch (phase)
        {
            case Phase.Searching:
                SearchForGrabbable();
                break;

            case Phase.Moving:
                MoveToTarget();
                break;
        }

        return Status.Running;
    }

    private void SearchForGrabbable()
    {
        
        Collider[] hits = Physics.OverlapSphere(selfTransform.position, radius);
        
        targetGrabbable = null;
    
        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value)
            {
                continue;
            }
    
            if (!hit.CompareTag(Object.Value))
            {
                continue;
            }
    
            if (hit.TryGetComponent(out IGrabbable grabbable))
            {
                // Cible trouvée
                targetGrabbable = grabbable;
                aiMove.SetDestination(hit.transform.position); 
                GrabbedObject.Value = hit.gameObject;
                phase = Phase.Moving; 
                
                return; 
            }
        }
        searchFailed = true; 
    }
    
    private void MoveToTarget()
    {
        if (targetGrabbable == null)
        {
            searchFailed = true;
            return;
        }

        if (!(targetGrabbable is MonoBehaviour targetMono))
        {
             searchFailed = true;
             return;
        }

        float distance = Vector3.Distance(selfTransform.position, targetMono.transform.position);
     


        if (distance <= 1.5f)
        {
            targetGrabbable.Grabbed(_controller);
            
            aiMove.Stop();
            
            phase = Phase.Done; 

        }
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }
}