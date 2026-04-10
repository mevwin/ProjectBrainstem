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

    public enum SplotchState
    {
        NONE,
        SPAWN,
        REPOSITION,
        DESPAWN
    }

    public Artist(Player player): base(player) { }

    private float hitDistance = 0f;
    private Vector3 hitPosition = Vector3.zero;
    private SplotchState splotchState = SplotchState.NONE;

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Activated Artist Ability");

        if (args != null)
        {
            if (args.ContainsKey("hitDistance"))
                hitDistance = (float) args["hitDistance"];
            
            if (args.ContainsKey("hitPosition"))
                hitPosition = (Vector3) args["hitPosition"];

            if (args.ContainsKey("splotchState"))
                splotchState = (SplotchState) args["splotchState"];
        }
        
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


                switch (splotchState)
                {
                    case SplotchState.SPAWN:
                        player.ArtistSpawnBlueSplotch(hitPosition);

                        break;
                    
                    case SplotchState.REPOSITION:
                        if (hitPosition != Vector3.zero)
                            player.ArtistRepositionBlueSplotch(hitPosition);

                        break;

                    case SplotchState.DESPAWN:
                        player.ArtistDespawnBlueSplotch();
                        
                        break;
                }

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
        player.ExitJobState();

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
        // Debug.Log("Exitted Artist Ability");
        player.abilityActive = false;
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
