using UnityEngine;

public class ProjectileNote : Entity
{
    Vector3 startPosition;
    protected override void InitializeStates() { }

    public override void Start()
    {
        base.Start();

        startPosition = transform.position - new Vector3(0, 2f, 0);
    }

    public override void Update()
    {
        base.Update();

        if ((startPosition - transform.position).magnitude > 20f)
        {
            // Debug.Log("Decay");
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name != "Player")
        { 
            collision.transform.gameObject.TryGetComponent(out Lever lever);
            if (lever != null)
                lever.Pickup(null);

            transform.position = new Vector3(0, -999f, 0);
            Destroy(gameObject, 0.5f);
        }
    }
}
