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

    const float blueSplotchDistanceCheck = 30f;

    private RaycastHit[] surfaces;
    private GameObject targetSurface;
    private GameObject blueSplotch = null;

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Activated Artist Ability");

        if (args != null)
        {
            // if (args.ContainsKey("hitDistance"))
            //     hitDistance = (float) args["hitDistance"];
            
            // if (args.ContainsKey("hitPosition"))
            //     hitPosition = (Vector3) args["hitPosition"];
        }
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Artist Ability State");
        

        switch (player.CurrentSplotch)
        {
            case Splotch.RED:
                
                break;

            case Splotch.BLUE:
                if (blueSplotch == null && ArtistCheckForBlueSplotchSpawn())
                {
                    ArtistSpawnBlueSplotch(targetSurface.transform.position);
                }
                else
                {
                    if (ArtistCheckForActiveBlueSplotch())
                        ArtistDespawnBlueSplotch();
                    else if (ArtistCheckForBlueSplotchSpawn())
                    {
                        ArtistRepositionBlueSplotch(targetSurface.transform.position);
                    }
                }
            
                break;
            
            case Splotch.YELL0W:
            
                break;
        }

        player.ExitJobState();
    }

    public override void FixedUpdateState()
    {
        // Debug.Log("Fixed Updating Artist Ability State");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Exitted Artist Ability");
        player.abilityActive = false;
    }

    /*
    if blue splotch is not been activated, spawn a new splotch. blue splotch must be spawned on top of a flat surface that's horizontally or vertically flat

    if blue splotch has been activated but player is not looking at it, reposition current splotch to new position

    if blue splotch has been activated and player is looking at it, despawn splotch
    */

    public void ArtistSpawnBlueSplotch(Vector3 position)
    {
        blueSplotch = GameObject.Instantiate(player.blueSplotchPrefab, position, Quaternion.identity);
    }

    public void ArtistRepositionBlueSplotch(Vector3 newPosition)
    {
        blueSplotch.transform.position = newPosition;
    }

    public void ArtistDespawnBlueSplotch()
    {
        Object.Destroy(blueSplotch);
        blueSplotch = null;
        Debug.Log("Destroyed Blue Splotch");
    }

    public bool ArtistCheckForBlueSplotchSpawn()
    {
        if (Physics.Raycast(
            player.cam.transform.position + player.boxCastOffset, 
            player.cam.transform.forward, 
            out RaycastHit hit, 
            30f)
        ) {   
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            if (dot > 0.99f) // Closest to 1.0 means flatter
            {
                targetSurface = hit.collider.gameObject;
                return true;
            }
        }
        return false;
    }

    public bool ArtistCheckForActiveBlueSplotch()
    {
        
        surfaces = player.ZoomDetection(blueSplotchDistanceCheck);

        foreach (RaycastHit surface in surfaces)
        {
            if (surface.collider.gameObject.TryGetComponent<BlueSplotch>(out _)
            )
                return true;
        }
        return false;
    }
}
