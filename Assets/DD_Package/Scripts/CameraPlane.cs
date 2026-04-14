using UnityEngine;
using UnityEngine.Rendering;

public class CameraPlane : MonoBehaviour
{
    public GameObject player;
    bool triggerMove = false;
    Vector3 dist;


    // Update is called once per frame
    void LateUpdate()
    {

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, -transform.localPosition.z + .1f);
        Debug.DrawRay(transform.position, transform.forward * (-transform.localPosition.z + .1f), Color.red, 1f);

        if (hit.collider != null)
        {

            if (hit.collider.gameObject != player)
            {
                Debug.Log(hit.collider.gameObject);
                transform.localPosition += (new Vector3(0, 0, 10) * Time.deltaTime);
            }

            else
            {
                //check if the camera is in an object

                Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, 1f);

                if (hit2.collider == null)
                {
                    if ((transform.position - player.transform.position).magnitude < 5.78)
                    {
                        transform.localPosition += (new Vector3(0, 0, -10) * Time.deltaTime);

                    }
                }
            }
        }
    }



}
