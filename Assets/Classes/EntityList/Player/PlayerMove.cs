using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    public PlayerMove(Player player): base(player) { }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        player.SetColliderStaticFriction(0f);
        player.SetColliderFrictionCombine(PhysicsMaterialCombine.Minimum);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        if (player.IsMoving())
        {
            Vector3 rotatedVector = player.gameObject.transform.rotation * player.GetMovementVector();
            Vector3 output = player.movementSpeed * rotatedVector.normalized + player.poleVaultBoost;

            player.UpdateMovementVector(output);
        }
        else player.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
