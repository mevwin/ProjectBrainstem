using System.Collections.Generic;
using UnityEngine;

public class Builder : JobState
{
    public Builder(Player player): base(player) { }

    private const int MAX_BLOCKS = 5;
    private const float MAX_DISTANCE = 10f;
    private const float forwardOffset = 0.5f;
    private readonly Vector3 spawnOffset = new(0f, 0.65f, 0f);

    private List<GameObject> blocks = new();
    Vector3 spawnPos;
    private float blockScale = 1.2f;

    
    public override void EnterState(Dictionary<string, object> args = null)
    {
        blocks.RemoveAll(item => item == null);
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
            GameObject block = SpawnBlock();
            blocks.Add(block);

            UpdateBlockAge();
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
            Object.Destroy(player.CurrentProjectedBlock);
            return;
        }

        if (hit.collider.gameObject.layer == 10 && player.WasInteractPressed())
            hit.collider.gameObject.GetComponent<BuilderBlock>().Despawn();
        
        spawnPos = hit.point + forwardOffset * blockScale * hit.normal;

        if (player.CurrentProjectedBlock)
            player.CurrentProjectedBlock.transform.position = spawnPos;
        else
        {
            player.CurrentProjectedBlock = Object.Instantiate(player.BlockProjection, spawnPos, Quaternion.identity);
            player.CurrentProjectedBlock.transform.localScale = Vector3.one * blockScale;
        }
    }

    void UpdateBlockAge()
    {
        if (blocks.Count > MAX_BLOCKS)
        {
            var oldest = blocks[0];
            blocks.RemoveAt(0);
            oldest.GetComponent<BuilderBlock>().Despawn();
        }
    }

    // Swaps between 1.2 and 2.4
    public void ChangeBlockSize()
    {
        blockScale = 3.6f - blockScale;
        Object.Destroy(player.CurrentProjectedBlock);
    }

    public GameObject SpawnBlock()
    {
        // projectile math to get it to land at spawnPos vv
        // GameObject block = Object.Instantiate(player.BlockBuilt, spawnPos, Quaternion.identity);

        // always spawn block at Player.transform.position + 1
        Vector3 startPosition = player.transform.position + Vector3.up;

        GameObject block = Object.Instantiate(player.BlockBuilt, startPosition, Quaternion.identity);
        block.transform.localScale = Vector3.one * blockScale;

        Rigidbody rb = block.GetComponent<Rigidbody>();

        // set the direction for horizontal velocity (minus 1/2 height of block)
        Vector3 towards = spawnPos - new Vector3(0, .6f, 0) - startPosition;
        //Debug.Log(towards);

        Vector3 horizontalDisplacement = new Vector3(towards.x, 0, towards.z);
        //Debug.Log(horizontalDisplacement);

        Debug.DrawRay(startPosition, horizontalDisplacement, Color.red, 2f);

        // Split Y trajectory into 2 parts:
        // 1. going up and returning to startPosition height
        // 2. going down from startPosition height to 0 with initial downwards velocity

        // y displacement
        float vertDisplacement = Mathf.Abs(towards.y);
        //Debug.Log(vertDisplacement);

        float G = 50;
        float initVUp = 10f;

        // add more height if the spot is above the lauch height --
        // I don't think I need to do the real math since we will not be aiming above head much

        if (vertDisplacement > 0)
        {
            initVUp += 2 * vertDisplacement;
        }

        // Part 1 (t1) returning to original y (vertDisp = 0)
        // quadratic formula for first time -- pre-reduced since C = 0
        float t1 = initVUp / G;
        float v1 = initVUp - (G * t1);

        // Part 2, original delta Y to ground, with starting velocity V1

        // dY = v0t + .5 g t^2
        float t2 = (-v1 - Mathf.Sqrt((v1 * v1) - (4 * -G * .5f * vertDisplacement)) / -G);
        //Debug.Log(t2);

        // horzontal disp = v0 * t | (vx constant through both parts of Y, so X's t value is t1 + t2)
        float initialVelocityX = horizontalDisplacement.magnitude / (t1 + t2);
        //Debug.Log(initialVelocityX);

        // send flying
        rb.AddForce(horizontalDisplacement.normalized * initialVelocityX + Vector3.up * initVUp, ForceMode.VelocityChange);
        // add torque
        rb.AddTorque(Random.onUnitSphere * 5000f);

        BuilderBlock blockScript = block.GetComponent<BuilderBlock>();
        blockScript.blocks = blocks;
        if (blockScale > 2f) blockScript.weight = Entity.Weight.HEAVY;

        blockScript.showTooltip = player.inTutorial;

        return block;
        // --------
    }
}
