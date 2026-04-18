using UnityEngine;

public class MusicNote : Entity
{
    Vector3 startPosition;
    public GameObject bridge;

    protected override void InitializeStates() { }

    public override void Start()
    {
        base.Start();

        startPosition = transform.position;
    }

    public override void Update()
    {
        base.Update();

        if ((startPosition - transform.position).magnitude > 20f)
        {
            Debug.Log("Decay");
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name != "Player")
        {
            Vector3 midpoint = (startPosition + transform.position) / 2;
            float length = (startPosition - transform.position).magnitude;
            GameObject bridgeObject = Object.Instantiate(bridge, midpoint, Quaternion.identity);
            bridgeObject.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, length);
            float yRot = Mathf.Atan2(transform.position.x - startPosition.x, transform.position.z - startPosition.z) * Mathf.Rad2Deg;
            float xRot = Mathf.Atan2(startPosition.y - transform.position.y, startPosition.z - transform.position.z) * Mathf.Rad2Deg;
            bridgeObject.transform.eulerAngles = new Vector3(xRot, yRot, 0f);
            Destroy(gameObject);
        }
    }
}
