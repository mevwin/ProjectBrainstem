using System.Collections.Generic;
using UnityEngine;

public class Builder : JobState
{
    public Builder(Player player): base(player) { }

    private float timer = 0f;


    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("Activated Builder Ability");
        timer = 0f;
    }

    public override void UpdateState()
    {
        Debug.Log("Updating Builder Ability State");
        if (timer > 2.0f) 
        {
            player.ExitJobState();
        }
    }

    public override void FixedUpdateState()
    {
        Debug.Log("Fixed Updating Builder Ability State");

        timer += Time.fixedDeltaTime;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        Debug.Log("Exitted Builder Ability");
    }
}
