using System.Collections.Generic;
using UnityEngine;

public class PlayerNoState : PlayerState
{
    public PlayerNoState(Player player): base(player) { }

    public override void EnterState(Dictionary<string, object> args = null) { }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void ExitState(Dictionary<string, object> args = null) { }
}
