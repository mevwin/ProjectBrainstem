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
    public RaycastHit selectedActiveSplotchHit;
    private GameObject bridgeNote;
    

    public override void EnterState(Dictionary<string, object> args = null) { }

    public override void UpdateState()
    {
        // Debug.Log("Updating Artist Ability State");

        if (CheckForActiveSplotch())
            DespawnSplotch();
        else if (CheckForDrawPosition())
        {
            if (splotches[currentSplotch] == null)
                SpawnSplotch(targetPosition);
            else
                RepositionSplotch(targetPosition);
        }

        player.ExitJobState();
    }

    public override void FixedUpdateState() { }

    public override void ExitState(Dictionary<string, object> args = null) { }

    void SpawnSplotch(Vector3 position)
    {
        GameObject prefab = currentSplotch switch
        {
            Splotch.RED => player.redSplotchPrefab,
            Splotch.BLUE => player.blueSplotchPrefab,
            _ => null,
        };

        if (prefab)
        {
            splotches[currentSplotch] = Object.Instantiate(prefab, position, targetRotation);
            ToggleSplotchBridgeParent();
        }
    }

    void RepositionSplotch(Vector3 newPosition)
    {
        splotches[currentSplotch].transform.SetPositionAndRotation(newPosition, targetRotation);
        ToggleSplotchBridgeParent();
    }

    void DespawnSplotch()
    {
        player.ignoreGravity = false;
        player.splotchMovement = Vector3.zero;
        GameObject obj = selectedActiveSplotchHit.collider.gameObject.transform.parent.gameObject;
        Object.Destroy(obj);
    }

    void ToggleSplotchBridgeParent()
    {
        if (bridgeNote)
            splotches[currentSplotch].transform.SetParent(bridgeNote.transform, true);
        else
            splotches[currentSplotch].transform.SetParent(null);
    }

    public bool CheckForDrawPosition()
    {
        if (Physics.Raycast(
            player.cam.transform.position, 
            player.cam.transform.forward, 
            out RaycastHit hit, 
            splotchDistanceCheck,
            player.artistSpawnCastMask)
        ) {   
            targetPosition = hit.point;
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (hit.collider.gameObject.layer == 8)
                bridgeNote = hit.collider.gameObject.transform.parent.gameObject;
            else
                bridgeNote = null;

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
            splotchDistanceCheck,
            player.artistDeleteCastMask);
    }

    public void CycleNextColor()
    {
        currentSplotch = (Splotch) (((int) currentSplotch + 1) % 2);
    }

    public Color SplotchTypeToColor()
    {
        Color color = Color.gray;

        if (CheckForActiveSplotch())
        {
            color = Color.magenta;
        }
        else if (CheckForDrawPosition())
        {
            color = currentSplotch switch
            {
                Splotch.RED => new Color32(0xff, 0x24, 0x00, 0xff),
                Splotch.BLUE => new Color32(0x10, 0x80, 0xfe, 0xff),
                _ => Color.gray
            };
        }
        return color;
    }
}