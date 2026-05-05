using UnityEngine;

public class BridgeNote : Entity
{
    Vector3 startPosition;
    public GameObject Bridge;
    public Musician musician;
    bool hit;

    protected override void InitializeStates() { }

    public override void Start()
    {
        base.Start();

        startPosition = transform.position - new Vector3(0f, 1.25f, 0f);
    }

    public override void Update()
    {
        base.Update();

        if ((startPosition - transform.position).magnitude > 20f)
        {
            CreateBridge();
        }
    }

    private void CreateBridge()
    {
        hit = true;
        Vector3 midpoint = (startPosition + transform.position) / 2;
        Vector3 diff = transform.position - startPosition;
        float length = diff.magnitude;
        GameObject bridgeParent = Instantiate(Bridge, midpoint, Quaternion.identity);
        GameObject bridgeObject = bridgeParent.transform.GetChild(0).gameObject;
        bridgeObject.transform.localScale = new Vector3(bridgeObject.transform.localScale.x, bridgeObject.transform.localScale.y, length);
        float yRot = Mathf.Asin(diff.x / Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(diff.z);
        float xRot = Mathf.Asin(diff.y / Mathf.Sqrt(Mathf.Pow(diff.y, 2) + Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(diff.z) * -1;
        xRot = Mathf.Clamp(xRot, -45f, 45f);
        bridgeObject.transform.eulerAngles = new Vector3(xRot, yRot, 0f);
        Musician.bridge = bridgeParent;

        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (hit) return;
        if (collision.transform.name != "Player")
        {
            CreateBridge();
        }
    }
}
