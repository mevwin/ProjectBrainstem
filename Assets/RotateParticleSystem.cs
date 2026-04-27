using UnityEngine;

public class RotateParticleSystem : MonoBehaviour
{
    public bool CCW = true;
    public float speed = 2f;

    private void Update()
    {
        if (!CCW)
        {
            transform.Rotate(Vector3.forward * speed);
        }
        else
        {
            transform.Rotate(-Vector3.forward * speed);
        }

    }


}
