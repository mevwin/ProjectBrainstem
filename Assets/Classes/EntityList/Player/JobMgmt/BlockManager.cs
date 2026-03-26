using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject BlockParent;
    [SerializeField] GameObject blockPrefab;
    [NonSerialized] public List<GameObject> blocks;

    private void Start()
    {
        blocks = new List<GameObject>();
    }

    public void UpdateBlocks()
    {
        foreach (Transform child in BlockParent.transform)
        {
            if (child.localScale.x < 3f)
            {
                child.localScale *= 1.1f + Time.deltaTime;
            }
        }
    }

    public void CreateBlock()
    {
        Physics.Raycast(player.transform.position, player.transform.GetChild(1).transform.forward, out RaycastHit hit, 2.5f);

        if (hit.collider == null)
        {
            if (blocks.Count > 4)
            {
                GameObject toDestroy = blocks[0];
                blocks.Remove(toDestroy);
                Destroy(toDestroy);

            }
            GameObject newBlock = Instantiate(blockPrefab, BlockParent.transform);
            newBlock.transform.localScale = Vector3.one * .3f;
            blocks.Add(newBlock);

            newBlock.transform.position = player.transform.position + Vector3.up * 2.5f;

            Launch(newBlock.GetComponent<MoveableCube>());
        }
        else if (hit.collider.gameObject.transform.parent == BlockParent.transform)
        {
            GameObject toDestroy = hit.collider.gameObject;
            blocks.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }

    void Launch(MoveableCube block)
    {
        block.AddForce(player.transform.forward * 40000f + Vector3.up * 50000f);
        block.AddTorque(Vector3.up * 5000f);
    }
}
