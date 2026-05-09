using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuilderBlock : Item
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyAnim;

    public List<GameObject> blocks = new List<GameObject>();

    public override void Update()
    {
        if (transform.position.y < -85f)
            Despawn();
    }

    public override void Pickup(Player player)
    {
        if (shot)
        {
            Drop();
            return;
        }
        else if (player.IsZoomHeld() && player.CurrentJob == JobManager.Job.BUILDER)
        {
            Despawn();
        }
        else
        {
            base.Pickup(player);
            Vector3 position = player.transform.position + player.cam.transform.forward * 3;
            Vector3 dir = position - transform.position;
            float mag = dir.magnitude;
            mag = Mathf.Clamp(mag, 0f, 10f);
            dir = 10 * mag * dir.normalized;
            rigidBody.linearVelocity = dir;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public override void Drop()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
    }

    public void Despawn()
    {
        blocks.Remove(gameObject);
        model.SetActive(false);

        gameObject.layer = 12;
        destroyAnim.SetActive(true);
        Destroy(gameObject, 1.23f);
    }
}
