using UnityEngine;

public class KitManager : MonoBehaviour
{
    public string currentKit;

    public GameObject[] Artist;
    public GameObject[] Athlete;
    public GameObject[] Builder;
    public GameObject[] Musician;

    public GameObject ParticleParent;

    

    public void SwitchKit(string toKit)
    {
        Debug.Log("Called: " +  toKit);
        currentKit = toKit;
        KitReset();

        foreach (Transform child in ParticleParent.transform) {
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
            child.GetComponent<ParticleSystem>().Play();
        }
        KitEnable(toKit);


    }

    void KitReset()
    {
        foreach (GameObject go in Artist)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in Athlete)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in Builder)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in Musician)
        {
            go.SetActive(false);
        }
    }

    public void KitEnable(string kitName)
    {
        switch (kitName)
        {
            case "Artist":
                foreach (GameObject go in Artist)
                {
                    go.SetActive(true);
                }
                break;
            case "Athlete":
                foreach (GameObject go in Athlete)
                {
                    go.SetActive(true);
                }
                break;
            case "Builder":
                foreach (GameObject go in Builder)
                {
                    go.SetActive(true);
                }
                break;
            case "Musician":
                foreach (GameObject go in Musician)
                {
                    go.SetActive(true);
                }
                break;
        }
    }
}
