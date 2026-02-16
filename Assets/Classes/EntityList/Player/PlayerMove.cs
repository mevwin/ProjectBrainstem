using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    public PlayerMove(Player player): base(player) { }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        if (player.IsMoving())
        {
            Vector3 rotatedVector = player.gameObject.transform.rotation * player.GetMovementVector();

            player.UpdateMovementVector(rotatedVector.normalized);
        }
        else player.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
