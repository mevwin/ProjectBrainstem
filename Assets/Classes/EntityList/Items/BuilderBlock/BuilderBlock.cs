using UnityEngine;

public class BuilderBlock : Item
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyAnim;

    public override void Pickup(Player player) { }

    public override void Drop() { }

    public void Despawn()
    {
        model.SetActive(false);
        destroyAnim.SetActive(true);
        Destroy(gameObject, 1.23f);
    }
}
