using System.Collections.Generic;
using UnityEngine;

public class Builder : JobState
{
    public Builder(Player player): base(player) { }

    private Queue<GameObject> blocks = new Queue<GameObject>();
    private int MAX_BLOCKS = 3;

    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        Physics.Raycast(player.gameObject.transform.position + new Vector3(0f, 0.5f, 0f), player.cam.transform.forward, out RaycastHit hit);
        if (hit.collider == null) return;
        if (hit.distance >= 4f) return;

        Vector3 spawnPos = hit.point + hit.normal * player.BlockBuilt.transform.localScale.y * 0.5f;

        GameObject block = Object.Instantiate(player.BlockBuilt, spawnPos, Quaternion.identity);
        blocks.Enqueue(block);

        if (blocks.Count > MAX_BLOCKS)
        {
            blocks.Dequeue().GetComponent<BuilderBlock>().Despawn();
        }
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
        Physics.Raycast(player.gameObject.transform.position + new Vector3(0f, 0.5f, 0f), player.cam.transform.forward, out RaycastHit hit);
        if (hit.collider == null) return;
        if (hit.distance >= 4f) return;

        Vector3 spawnPos = hit.point + hit.normal * player.BlockProjection.transform.localScale.y * 0.5f;

        Object.Instantiate(player.BlockProjection, spawnPos, Quaternion.identity);
    }
}
