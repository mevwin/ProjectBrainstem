using System.Collections.Generic;
using UnityEngine;

public class BuilderBlock : Item
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyAnim;
    [SerializeField] private LayerMask pickupExcludeLayers;

    public List<GameObject> blocks = new();

    public override void Update()
    {
        if (transform.position.y < -55f)
            Despawn();
    }

    public override void Pickup(Player player)
    {
        if (shot)
            Drop();
        else if (player.IsZoomHeld() && player.CurrentJob == JobManager.Job.BUILDER)
            Despawn();
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

            rigidBody.excludeLayers = pickupExcludeLayers;
        }
    }

    public override void Drop()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.excludeLayers = 0;
    }

    public void Despawn()
    {
        blocks.Remove(gameObject);
        destroyAnim.SetActive(true);
        destroyAnim.transform.parent = null;
        Destroy(destroyAnim, 1.23f);

        transform.position = new Vector3(0, -999f, 0);
        Destroy(gameObject, 0.1f);
    }
}
