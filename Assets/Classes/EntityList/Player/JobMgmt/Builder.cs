using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Builder : JobState
{
    public Builder(Player player): base(player) { }

    private const int MAX_BLOCKS = 3;
    private const float MAX_DISTANCE = 10f;
    private const float forwardOffset = 0.5f;
    private readonly Vector3 spawnOffset = new(0f, 0.65f, 0f);

    private Queue<GameObject> blocks = new();
    private GameObject projectBlock = null;
    Vector3 spawnPos;

    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        Physics.Raycast(
            player.transform.position + spawnOffset, 
            player.cam.transform.forward, 
            out RaycastHit hit,
            MAX_DISTANCE
        );
        if (hit.collider == null) 
            return;
        // else if (blocks.Contains(hit.collider.gameObject)) // TODO: add another button to destroy existing button
        // {
        //     GameObject block = hit.collider.gameObject;

        //     var tempList = blocks.ToList();

        //     block.GetComponent<BuilderBlock>().Despawn();
        //     tempList.Remove(block);
            
        //     blocks = new Queue<GameObject>(tempList);
        // }
        else
        {
            GameObject block = Object.Instantiate(player.BlockBuilt, spawnPos, Quaternion.identity);
            blocks.Enqueue(block);

            if (blocks.Count > MAX_BLOCKS)
                DestroyOldestBlock();
        }
    }

    public override void UpdateState()
    {
        player.ExitJobState();
    }

    public override void FixedUpdateState() { }

    public override void ExitState(Dictionary<string, object> args = null) { }

    public void ProjectBlock()
    {
        Physics.Raycast(
            player.transform.position + spawnOffset, 
            player.cam.transform.forward, 
            out RaycastHit hit,
            MAX_DISTANCE
        );
        if (hit.collider == null)
        {
            if (projectBlock)
                Object.Destroy(projectBlock);
            return;
        }
        
        spawnPos = hit.point + forwardOffset * player.BlockProjection.transform.localScale.y * hit.normal;

        if (projectBlock)
            projectBlock.transform.position = spawnPos;
        else
            projectBlock = Object.Instantiate(player.BlockProjection, spawnPos, Quaternion.identity);
    }

    void DestroyOldestBlock()
    {
        blocks.Dequeue().GetComponent<BuilderBlock>().Despawn();
    }
}
