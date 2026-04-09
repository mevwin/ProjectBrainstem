using System.Collections.Generic;
using UnityEngine;

public class Artist : JobState
{
    public enum Splotch
    {
        NONE,
        RED,
        BLUE,
        YELL0W
    }

    public Artist(Player player): base(player) { }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Activated Artist Ability");
        
        switch (player.CurrentSplotch)
        {
            case Splotch.RED:

                break;

            case Splotch.BLUE:
                /*
                if blue splotch is not been activated, spawn a new splotch. blue splotch must be spawned on top of a flat surface that's horizontally or vertically flat

                if blue splotch has been activated but player is not looking at it, reposition current splotch to new position

                if blue splotch has been activated and player is looking at it, despawn splotch
                */

                

                break;
            
            case Splotch.YELL0W:
                
                break;

            default:
                player.ExitJobState();
                return;
        }
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Artist Ability State");
        switch (player.CurrentSplotch)
        {
            case Splotch.RED:
                RedPaintEffectUpdate();
                break;

            case Splotch.BLUE:
                BluePaintEffectUpdate();
                break;
            
            case Splotch.YELL0W:
                YellowPaintEffectUpdate();
                break;

            default:
                return;
        }
    }

    public override void FixedUpdateState()
    {
        // Debug.Log("Fixed Updating Artist Ability State");

        switch (player.CurrentSplotch)
        {
            case Splotch.RED:
                RedPaintEffectFixedUpdate();
                break;

            case Splotch.BLUE:
                BluePaintEffectFixedUpdate();
                break;
            
            case Splotch.YELL0W:
                YellowPaintEffectFixedUpdate();
                break;

            default:
                return;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        Debug.Log("Exitted Artist Ability");
    }

    // Stop vertical movement for interactables
    private void RedPaintEffectUpdate()
    {
        
    }

    private void RedPaintEffectFixedUpdate()
    {
        
    }

    // Make directional elevator that slowly levitates an object in it in the normal direction
    private void BluePaintEffectUpdate()
    {
        
    }

    private void BluePaintEffectFixedUpdate()
    {
        
    }
    
    // Make specific surfaces and interactables bouncy
    private void YellowPaintEffectUpdate()
    {
        
    }

    private void YellowPaintEffectFixedUpdate()
    {
        
    }
}
