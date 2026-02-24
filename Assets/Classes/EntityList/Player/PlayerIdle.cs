using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Player player): base(player) { }

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
        player.UpdateMovementVector(new Vector3(0, player.GetRigidbodyVelocity().y, 0f));
        if (player.IsMoving()){
            player.ChangeState("Move");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
