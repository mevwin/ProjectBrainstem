using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject BlockParent;
    public GameObject Block;
    public List<GameObject> blocks;
    public GameObject destructParticles;
    public GameObject createParticles;

    private void Start()
    {
        blocks = new List<GameObject>();
    }
void Update()
    {

        if(Input.GetKeyDown(KeyCode.F))
        {
            CreateBlock();
        }

        foreach (Transform child in BlockParent.transform)
        {
            if (child.localScale.x < 3f)
            {
                child.localScale *= 1.1f + Time.deltaTime;
            }
        }


    }

    void CreateBlock()
    {
        Physics.Raycast(Player.transform.position, Player.transform.GetChild(1).transform.forward, out RaycastHit hit, 2.5f);

        if (hit.collider == null)
        {
            if (blocks.Count >4)
            {
                GameObject toDestroy = blocks[0];
                blocks.Remove(toDestroy);
                GameObject remove = Instantiate(destructParticles, toDestroy.transform.position, Quaternion.Euler(-90, 0, 0));
                StartCoroutine(GarbageCollect(remove));
                Destroy(toDestroy);

            }
            GameObject newBlock = Instantiate(Block, BlockParent.transform);
            newBlock.transform.localScale = Vector3.one * .3f;
            blocks.Add(newBlock);
           


            newBlock.transform.position = Player.transform.position + Vector3.up *2f;
            GameObject killThisGuy = Instantiate(createParticles, newBlock.transform.position, Quaternion.Euler(-90, 0, 0));
            StartCoroutine(GarbageCollect(killThisGuy));

            RandomLaunch(newBlock);
            //Player.transform.GetChild(1).transform.forward;
        }
        else if (hit.collider.gameObject.transform.parent == BlockParent.transform)
        {
            Debug.Log("Check1");
            GameObject toDestroy = hit.collider.gameObject;
            GameObject killThisGuy = Instantiate(destructParticles, toDestroy.transform.position, Quaternion.Euler(-90, 0 ,0));
            StartCoroutine(GarbageCollect(killThisGuy));

            blocks.Remove(toDestroy);
            Destroy(toDestroy);

        }


    }


    void RandomLaunch(GameObject block)
    {
        Vector3 randomPoint = Random.insideUnitCircle;
        Debug.Log(randomPoint);
        Vector3 pos = new Vector3(randomPoint.x, 0 , randomPoint.y);

        block.GetComponent<Rigidbody>().AddForce((Player.transform.GetChild(1).transform.forward + pos * 60000f) + Vector3.up * 50000f);
        block.GetComponent<Rigidbody>().AddTorque(Vector3.up * 10000f);
    }

    IEnumerator GarbageCollect(GameObject toDestroy)
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(toDestroy);
    }
}
