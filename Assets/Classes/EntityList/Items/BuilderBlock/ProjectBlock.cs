using UnityEngine;

public class ProjectBlock : MonoBehaviour
{
    // If we are having performance issues, this is a good thing to check
    // Blocks are spawned and deleted every frame
    void Update()
    {
        Destroy(gameObject);
    }
}
