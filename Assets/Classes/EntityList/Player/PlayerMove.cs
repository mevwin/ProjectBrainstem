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
            Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.cam.transform.forward, Vector3.up).normalized);
            Vector3 rotatedVector = rotation * player.GetMovementVector();

            float speed = player.CurrentJob == JobManager.Job.ATHLETE && !player.abilityActive ? 
                            player.movementSpeed * player.athleteSpeedBoost : player.movementSpeed; 
            Vector3 output = speed * rotatedVector.normalized + player.poleVaultBoost + player.splotchMovement;

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
