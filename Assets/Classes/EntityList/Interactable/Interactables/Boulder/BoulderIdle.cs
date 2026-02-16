using System.Collections.Generic;
using UnityEngine;

public class BoulderIdle : InteractableState
{
    public BoulderIdle(Boulder boulder): base(boulder) { }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        interactable.UpdateMovementVector(Vector3.zero);
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

