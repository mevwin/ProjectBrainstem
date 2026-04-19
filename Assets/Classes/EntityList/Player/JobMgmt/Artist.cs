using System.Collections.Generic;
using UnityEngine;

public class Artist : JobState
{
    public enum Splotch
    {
        RED,
        BLUE,
        YELL0W
    }

    public Artist(Player player): base(player) { }

    const float blueSplotchDistanceCheck = 30f;

    // Raycasting Vars
    private RaycastHit[] surfaces;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Splotch currentSplotch = Splotch.BLUE;
    public GameObject blueSplotch = null;

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Activated Artist Ability");
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Artist Ability State");
        

        switch (currentSplotch)
        {
            case Splotch.RED:
                
                break;

            case Splotch.BLUE:
                if (blueSplotch == null && CheckForBlueSplotchSpawn())
                    SpawnBlueSplotch(targetPosition);
                else if (CheckForActiveBlueSplotch())
                    DespawnBlueSplotch();
                else if (CheckForBlueSplotchSpawn())
                    RepositionBlueSplotch(targetPosition);
            
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
    }

    /*
    if blue splotch is not been activated, spawn a new splotch. blue splotch must be spawned on top of a flat surface that's horizontally or vertically flat

    if blue splotch has been activated but player is not looking at it, reposition current splotch to new position

    if blue splotch has been activated and player is looking at it, despawn splotch
    */
    void SpawnBlueSplotch(Vector3 position)
    {
        blueSplotch = Object.Instantiate(player.blueSplotchPrefab, position, targetRotation);
    }

    void RepositionBlueSplotch(Vector3 newPosition)
    {
        blueSplotch.transform.SetPositionAndRotation(newPosition, targetRotation);
    }

    void DespawnBlueSplotch()
    {
        player.ignoreGravity = false;
        player.blueSplotchHorizMovement = Vector3.zero;
        Object.Destroy(blueSplotch);
        blueSplotch = null;
    }

    public bool CheckForBlueSplotchSpawn()
    {
        if (Physics.Raycast(
            player.cam.transform.position, 
            player.cam.transform.forward, 
            out RaycastHit hit, 
            blueSplotchDistanceCheck)
        ) {   
            float dot = Vector3.Dot(hit.normal, Vector3.up);

            if (dot > 0.99f || (dot > -0.05f && dot < 0.05f)) 
            {
                targetPosition = hit.point;
                targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                return true;
            }
        }
        return false;
    }

    public bool CheckForActiveBlueSplotch()
    {
        surfaces = player.ZoomDetection(blueSplotchDistanceCheck);

        foreach (RaycastHit surface in surfaces)
        {
            if (surface.collider.gameObject.TryGetComponent<BlueSplotch>(out _))
                return true;
        }
        return false;
    }

    public override bool IsReticleHittingSurface()
    {
        return currentSplotch switch
        {
            Splotch.BLUE => CheckForBlueSplotchSpawn(),
            _ => false,
        };
    }

    public void CycleNextColor()
    {
        currentSplotch = (Splotch) (((int) currentSplotch + 1) % 3);
    }

    public Color SplotchTypeToColor(bool hittingSurface)
    {
        Color color = Color.gray;

        if (hittingSurface)
        {
            color = currentSplotch switch
            {
                Splotch.RED => new Color32(0xff, 0x24, 0x00, 0xef),
                Splotch.BLUE => new Color32(0x00, 0x80, 0xfe, 0xef),
                Splotch.YELL0W => new Color32(0xf9, 0xe0, 0x76, 0xef),
                _ => Color.gray
            };
        }
        return color;
    }
}
