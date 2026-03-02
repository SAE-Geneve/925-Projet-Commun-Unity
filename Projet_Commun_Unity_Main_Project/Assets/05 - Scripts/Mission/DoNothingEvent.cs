using UnityEngine;

public class DoNothingEvent : GameEvent
{
    public override bool IsEventActive() 
    { 
        return false; 
    }

    public override void TriggerEvent() 
    {
        Debug.Log("Lancer de dés... Ouf ! Aucun événement cette fois-ci.");
    }

    public override void ResetEvent() 
    { 
    }
}