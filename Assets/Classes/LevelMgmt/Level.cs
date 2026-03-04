using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform playerSpawnPoint;
    protected GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        gameManager = GameManager.GetManager();

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void Start()
    {
        if (gameManager && gameManager.player){
            gameManager.player.SetActive(true);
            gameManager.player.transform.position = playerSpawnPoint.position;   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
