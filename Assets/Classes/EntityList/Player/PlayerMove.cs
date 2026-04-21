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

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        if (player.IsMoving())
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.cam.transform.forward, Vector3.up));
            Vector3 rotatedVector = rotation * player.GetMovementVector();
            Vector3 output = player.movementSpeed * rotatedVector.normalized + player.poleVaultBoost + player.splotchMovement;

            if (player.ignoreGravity)
                output.y = Mathf.Max(1f, player.splotchMovement.y + player.poleVaultBoost.y);

            player.UpdateMovementVector(output, player.ignoreGravity);
        }
        else player.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
