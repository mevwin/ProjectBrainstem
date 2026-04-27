using UnityEngine;

public class BuilderBlock : Item
{
    [SerializeField] private Collider boxCollider;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyAnim;

    public override void Pickup(Player player)
    {
        Despawn();
    }

    public override void Drop() { }

    public void Despawn()
    {
        model.SetActive(false);
        boxCollider.enabled = false;
        destroyAnim.SetActive(true);
        Destroy(gameObject, 1.23f);
    }
}
