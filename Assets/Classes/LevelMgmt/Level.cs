using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform playerSpawnPoint;
    [SerializeField] private ExitTrigger exitTrigger;
    protected GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        gameManager = GameManager.GetManager();

        Cursor.lockState = CursorLockMode.Locked;

        if (gameManager && gameManager.player){
            gameManager.player.SetActive(true);
            gameManager.player.transform.position = playerSpawnPoint.position;   
            gameManager.player.transform.rotation = playerSpawnPoint.rotation;
        }
    }
}
