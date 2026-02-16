using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : Entity
{
    [SerializeField] private bool isActive = false;

    // public abstract void Activated(Dictionary<string, object> args = null);
}
