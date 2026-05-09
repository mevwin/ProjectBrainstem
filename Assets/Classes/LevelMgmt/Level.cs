using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform playerSpawnPoint;
    [SerializeField] private float depthToRespawn = 0f;
    [SerializeField] Player player;

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

    protected virtual void Update()
    {
        if (player && player.transform.position.y < depthToRespawn)
        {
            if (gameManager && !gameManager.respawningPlayer)
            {
                gameManager.respawningPlayer = true;
                StartCoroutine(RespawnPlayer());
            }
            else // for debugging in editor
            {
                player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
                player.UpdateMovementVector(Vector3.zero, true);
            }
        }
    }

    private IEnumerator RespawnPlayer()
    {
        gameManager.loadingScreen.gameObject.SetActive(true);
        player.inMenu = true;
        yield return StartCoroutine(gameManager.loadingScreen.Fade(1f, 1f));

        player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        player.UpdateMovementVector(Vector3.zero, true);

        yield return StartCoroutine(gameManager.loadingScreen.Fade(0f, 2f));
        player.inMenu = false;
        gameManager.loadingScreen.gameObject.SetActive(false);
        gameManager.respawningPlayer = false;
    }
}
