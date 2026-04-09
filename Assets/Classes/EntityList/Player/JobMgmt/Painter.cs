using System.Collections.Generic;
using UnityEngine;

public class Painter : JobState
{
    public enum Splotch
    {
        NONE,
        RED,
        BLUE,
        YELL0W
    }

    public Painter(Player player): base(player) { }

    private float timer = 0f;

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("Activated Painter Ability");
        timer = 0f;
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Painter Ability State");
        if (timer > 2.0f) 
        {
            player.ExitJobState();
        }
    }

    public override void FixedUpdateState()
    {
        // Debug.Log("Fixed Updating Painter Ability State");

        timer += Time.fixedDeltaTime;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        Debug.Log("Exitted Painter Ability");
    }

    // Stop vertical movement for interactables
    private void RedPaintEffect()
    {
        
    }

    // Make directional elevator that slowly levitates an object in it in the normal direction
    private void BluePaintEffect()
    {
        
    }
    
    // Make specific surfaces and interactables bouncy
    private void YellowPaintEffect()
    {
        
    }
}
