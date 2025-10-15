
using UnityEngine;

public interface IGrabbable
{
    void Grabbed(PlayerController playerController);

    void Dropped(Vector3 throwForce = default, PlayerController playerController = null);
}
