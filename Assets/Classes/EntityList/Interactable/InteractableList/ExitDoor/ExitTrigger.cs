using System;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [NonSerialized] public bool isActive = false;

    public void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Player Exited Level");
        }
    }
}
