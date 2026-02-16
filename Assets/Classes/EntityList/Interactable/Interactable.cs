using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : Entity
{
    [SerializeField] protected bool isActive = false;

    public virtual void DetectActivation()
    {
        if (isActive)
        {
            Activated();
        }
    }

    public virtual void Activated(Dictionary<string, object> args = null)
    {
        Debug.Log("This is Active");
    }
}
