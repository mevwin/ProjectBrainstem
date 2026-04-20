using System.Collections.Generic;
using UnityEngine;

public class Artist : JobState
{
    public enum Splotch
    {
        RED,
        BLUE,
        YELLOW
    }

    public Artist(Player player): base(player) { }

    const float splotchDistanceCheck = 30f;

    // Raycasting Vars
    private RaycastHit[] surfaces;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Splotch currentSplotch = Splotch.BLUE;
    public GameObject splotch = null;

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
                if (splotch == null && CheckForSplotchSpawn())
                    SpawnSplotch(targetPosition);
                else if (CheckForActiveSplotch())
                    DespawnSplotch();
                else if (CheckForSplotchSpawn())
                    RepositionSplotch(targetPosition);
            
                break;
            
            case Splotch.YELLOW:
            
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
    void SpawnSplotch(Vector3 position)
    {
        GameObject prefab = currentSplotch switch
        {
            Splotch.RED => player.redSplotchPrefab,
            Splotch.BLUE => player.blueSplotchPrefab,
            _ => player.blueSplotchPrefab,
        };

        splotch = Object.Instantiate(prefab, position, targetRotation);
    }

    void RepositionSplotch(Vector3 newPosition)
    {
        splotch.transform.SetPositionAndRotation(newPosition, targetRotation);
    }

    void DespawnSplotch()
    {
        player.ignoreGravity = false;
        player.splotchMovement = Vector3.zero;
        Object.Destroy(splotch);
        splotch = null;
    }

    public bool CheckForSplotchSpawn()
    {
        if (Physics.Raycast(
            player.cam.transform.position, 
            player.cam.transform.forward, 
            out RaycastHit hit, 
            splotchDistanceCheck) &&
            hit.collider.gameObject.layer != 3
        ) {   
            // float dot = Vector3.Dot(hit.normal, Vector3.up);

            // if (dot > 0.99f || (dot > -0.05f && dot < 0.05f)) 
            // {
            targetPosition = hit.point;
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            return true;
            //}
        }
        return false;
    }

    public bool CheckForActiveSplotch()
    {
        surfaces = player.ZoomDetection(splotchDistanceCheck);

        foreach (RaycastHit surface in surfaces)
        {
            if (
                (currentSplotch == Splotch.BLUE && surface.collider.gameObject.TryGetComponent<BlueSplotch>(out _)) ||
                (currentSplotch == Splotch.RED && surface.collider.gameObject.TryGetComponent<RedSplotch>(out _))
            )
                return true;
        }
        return false;
    }

    public override bool IsReticleHittingSurface()
    {
        return currentSplotch switch
        {
            Splotch.BLUE => CheckForSplotchSpawn(),
            _ => false,
        };
    }

    public void CycleNextColor()
    {
        currentSplotch = (Splotch) (((int) currentSplotch + 1) % 3);
    }

    public Color SplotchTypeToColor()
    {
        Color color = Color.gray;

        if (IsReticleHittingSurface())
        {
            color = currentSplotch switch
            {
                Splotch.RED => new Color32(0xff, 0x24, 0x00, 0xef),
                Splotch.BLUE => new Color32(0x00, 0x80, 0xfe, 0xef),
                Splotch.YELLOW => new Color32(0xf9, 0xe0, 0x76, 0xef),
                _ => Color.gray
            };
        }
        return color;
    }
}
