using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Player player): base(player) { }

    Vector3 output = new();

    public override void EnterState(Dictionary<string, object> args = null)
    {
        player.SetColliderStaticFriction(1f);
        player.SetColliderFrictionCombine(PhysicsMaterialCombine.Maximum);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        output = new(player.poleVaultBoost.x, 
                     player.GetRigidbodyVelocity().y + player.poleVaultBoost.y, 
                     player.poleVaultBoost.z);
        player.UpdateMovementVector(output);
        if (player.IsMoving()){
            player.ChangeState("Move");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
