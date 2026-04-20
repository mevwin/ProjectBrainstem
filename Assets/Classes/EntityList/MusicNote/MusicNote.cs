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
            float yRot = Mathf.Asin((transform.position.x - startPosition.x) / Mathf.Sqrt(Mathf.Pow(transform.position.x - startPosition.x, 2) + Mathf.Pow(transform.position.z - startPosition.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(transform.position.z - startPosition.z);
            float xRot = Mathf.Asin((transform.position.y - startPosition.y) / Mathf.Sqrt(Mathf.Pow(transform.position.y - startPosition.y, 2) + Mathf.Pow(transform.position.z - startPosition.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(transform.position.z - startPosition.z) * -1;
            bridgeObject.transform.eulerAngles = new Vector3(xRot, yRot, 0f);

            Destroy(gameObject);
        }
    }
}
