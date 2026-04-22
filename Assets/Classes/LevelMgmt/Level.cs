using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform playerSpawnPoint;
    [SerializeField] private ExitTrigger exitTrigger;
    [SerializeField] private float depthToRespawn = 0f;
    protected GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        gameManager = GameManager.GetManager();

        Cursor.lockState = CursorLockMode.Locked;

        if (gameManager && gameManager.player){
            gameManager.player.SetActive(true);
            gameManager.player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }

    void Update()
    {
        if (gameManager.player && gameManager.player.transform.position.y < depthToRespawn)
        {
            gameManager.player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
            gameManager.player.GetComponent<Player>().UpdateMovementVector(Vector3.zero, true);
        }
    }
}
