using System.Collections.Generic;
using UnityEngine;

public class Builder : JobState
{
    public Builder(Player player): base(player) { }

    private float timer = 0f;

    private int counter = 0;

    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        Physics.Raycast(player.gameObject.transform.position, player.cam.transform.forward, out RaycastHit hit);
        if (hit.collider == null) return;
        if (hit.distance >= 4f) return;

        Vector3 spawnPos = hit.point + hit.normal * player.BlockBuilt.transform.localScale.y * 0.5f;

        Object.Instantiate(player.BlockBuilt, spawnPos, Quaternion.identity);
    }

    public override void UpdateState()
    {
        player.ExitJobState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

    public void ProjectBlock()
    {
        Physics.Raycast(player.gameObject.transform.position, player.cam.transform.forward, out RaycastHit hit);
        if (hit.collider == null) return;
        if (hit.distance >= 4f) return;

        Vector3 spawnPos = hit.point + hit.normal * player.BlockProjection.transform.localScale.y * 0.5f;

        Object.Instantiate(player.BlockProjection, spawnPos, Quaternion.identity);
    }
}
