using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private List<IRespawnable> _respawnables;

    private void Start()
    {
        _respawnables = new List<IRespawnable>();

        foreach (GameObject child in transform)
        {
            if (child.TryGetComponent<IRespawnable>(out var respawnable))
                _respawnables.Add(respawnable);
        }
    }

    public void RespawnObjects()
    {
        foreach (var r in _respawnables)
            r.Respawn();
    }
}
