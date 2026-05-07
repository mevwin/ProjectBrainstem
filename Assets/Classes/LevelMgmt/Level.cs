using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform playerSpawnPoint;
    [SerializeField] private float depthToRespawn = 0f;
    private Player player;
    protected GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        gameManager = GameManager.GetManager();

        Cursor.lockState = CursorLockMode.Locked;

        if (gameManager && gameManager.player){
            gameManager.player.SetActive(true);

            gameManager.TogglePlayerControls(true);

            player = gameManager.player.GetComponent<Player>();
            player.SetCurrentJob(JobManager.Job.NONE);
            player.SetStoredJob(JobManager.Job.NONE);
            player.UpdateMovementVector(Vector3.zero, true);
            player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
            player.playerHud.iconManager1.SetIcon(JobManager.Job.NONE);
            player.playerHud.iconManager2.SetIcon(JobManager.Job.NONE);
        }
    }

    void Update()
    {
        if (player && player.transform.position.y < depthToRespawn)
        {
            player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
            player.UpdateMovementVector(Vector3.zero, true);
        }
    }
}
