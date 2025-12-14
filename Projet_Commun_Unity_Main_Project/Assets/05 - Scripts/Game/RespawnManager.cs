using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private List<IRespawnable> _respawnables;

    private void Start()
    {
        _respawnables = new List<IRespawnable>();

        foreach (var r in GetComponentsInChildren<IRespawnable>())
            _respawnables.Add(r);
    }

    public void RespawnObjects()
    {
        foreach (var r in _respawnables)
            r.Respawn();
    }
}
