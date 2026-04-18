using Unity.VisualScripting;
using UnityEngine;

public class JobOrb : MonoBehaviour
{
    public GameObject Artist;
    public GameObject Musician;
    public GameObject Buiilder;
    public GameObject Athlete;

    public KitManager kitManager;

    void Awake()
    {
        kitManager = FindAnyObjectByType<KitManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // check player tag / layer vv
        //

        foreach (Transform child in transform)
        {

            if (child.gameObject.activeSelf)
            {
                ChangeJob(child.gameObject.name);
            }
        }
        

        gameObject.SetActive(false);

    }

    public void ChangeJob(string job)
    {
        kitManager.SwitchKit(job);
    }

}
