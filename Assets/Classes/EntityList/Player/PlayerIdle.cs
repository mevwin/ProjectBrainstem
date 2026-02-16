using System.Collections.Generic;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Player player): base(player) { }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        player.UpdateMovementVector(new(0, player.GetRigidbodyVelocity().y, 0f));
        if (player.IsMoving()){
            player.ChangeState("Move");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
