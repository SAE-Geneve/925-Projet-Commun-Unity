

using UnityEngine;

public interface IGrabbable
{
    void Grabbed(Transform grabbedBy);

    void Dropped();
}
