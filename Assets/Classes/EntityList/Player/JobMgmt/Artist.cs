using System.Collections.Generic;
using UnityEngine;

public class Artist : JobState
{
    public enum Splotch
    {
        RED,
        BLUE,
        NONE,
    }
    
    private Dictionary<Splotch, GameObject> splotches = new();
    public Artist(Player player): base(player)
    {
        splotches.Add(Splotch.RED, null);
        splotches.Add(Splotch.BLUE, null);
        splotches.Add(Splotch.NONE, null);
    }

    const float splotchDistanceCheck = 30f;

    // Raycasting Vars
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Splotch currentSplotch = Splotch.BLUE;
    private RaycastHit selectedActiveSplotchHit;
    

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // Debug.Log("Activated Artist Ability");
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Artist Ability State");

        if (splotches[currentSplotch] == null && CheckForDrawPosition())
            SpawnSplotch(targetPosition);
        else if (CheckForActiveSplotch())
            DespawnSplotch();
        else if (CheckForDrawPosition())
            RepositionSplotch(targetPosition);
        
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
    if splotch is not been activated, spawn a new splotch. blue splotch must be spawned on top of a flat surface that's horizontally or vertically flat

    if splotch has been activated but player is not looking at it, reposition current splotch to new position

    if splotch has been activated and player is looking at it, despawn splotch
    */
    void SpawnSplotch(Vector3 position)
    {
        GameObject prefab = currentSplotch switch
        {
            Splotch.RED => player.redSplotchPrefab,
            Splotch.BLUE => player.blueSplotchPrefab,
            _ => null,
        };

        if (prefab)
            splotches[currentSplotch] = Object.Instantiate(prefab, position, targetRotation);
    }

    void RepositionSplotch(Vector3 newPosition)
    {
        splotches[currentSplotch].transform.SetPositionAndRotation(newPosition, targetRotation);
    }

    void DespawnSplotch()
    {
        player.ignoreGravity = false;
        player.splotchMovement = Vector3.zero;
        GameObject obj = selectedActiveSplotchHit.collider.gameObject.transform.parent.gameObject;
        Object.Destroy(obj);
    }

    public bool CheckForDrawPosition()
    {
        if (Physics.Raycast(
            player.cam.transform.position, 
            player.cam.transform.forward, 
            out RaycastHit hit, 
            splotchDistanceCheck,
            player.artistCastMask)
        ) {   
            targetPosition = hit.point;
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            return true;
        }
        return false;
    }

    public bool CheckForActiveSplotch()
    {
        return Physics.Raycast(
            player.cam.transform.position, 
            player.cam.transform.forward, 
            out selectedActiveSplotchHit, 
            splotchDistanceCheck) &&
            selectedActiveSplotchHit.collider.gameObject.layer == 3;
    }

    public void CycleNextColor()
    {
        currentSplotch = (Splotch) (((int) currentSplotch + 1) % 2);
    }

    public Color SplotchTypeToColor()
    {
        Color color = Color.gray;

        if (CheckForDrawPosition())
        {
            color = currentSplotch switch
            {
                Splotch.RED => new Color32(0xff, 0x24, 0x00, 0xef),
                Splotch.BLUE => new Color32(0x00, 0x80, 0xfe, 0xef),
                _ => Color.gray
            };
        }
        else if (CheckForActiveSplotch())
        {
            color = Color.magenta;
        }
        return color;
    }

    Splotch GetSplotchType(GameObject obj)
    {
        if (obj.TryGetComponent<BlueSplotch>(out _))
            return Splotch.BLUE;
        else if (obj.TryGetComponent<RedSplotch>(out _))
            return Splotch.RED;
        else
            return Splotch.NONE;
    }
}