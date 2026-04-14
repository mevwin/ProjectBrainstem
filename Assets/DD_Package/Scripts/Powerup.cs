using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    GameObject child;
    bool wait = false;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (child.activeSelf)
        {

        }
        else
        {
            if (!wait)
            {
                wait = true;
                StartCoroutine(Refresh());
            }
        }
    }


    IEnumerator Refresh()
    {
        yield return new WaitForSecondsRealtime(3f);
        child.SetActive(true);
        wait = false;
    }
}
