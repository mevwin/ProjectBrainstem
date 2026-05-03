using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Transform CameraOffsetY;
    public GameObject mainCamera;
    [SerializeField] private float sensitivity, lowerBound, upperBound, distance;

    InputAction look;
    Vector2 lookAmt;

    float vertRotation = 0;
    float horRotation = 0;

    void Start()
    {
        // Get InputAction references from Project-wide input actions.
        if (InputSystem.actions)
        {
            look = InputSystem.actions.FindAction("Player/Look");
        }
    }

    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            lookAmt = look.ReadValue<Vector2>();
            Rotating();
            Positioning();
        }
    }

    private void Rotating()
    {
        if (lookAmt.x != 0f)
        {
            lookAmt.x *= sensitivity;
            horRotation += lookAmt.x;

        }

        if (lookAmt.y != 0f)
        {
            lookAmt.y *= sensitivity;

            vertRotation = Mathf.Clamp(vertRotation - lookAmt.y, lowerBound, upperBound);

            
        }
        CameraOffsetY.transform.localRotation = Quaternion.Euler(vertRotation, horRotation, 0);
    }

    private void Positioning()
    {
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0f, 2f, 0f), mainCamera.transform.forward * -1, out RaycastHit hit))
        {
            if (hit.distance < distance)
            {
                CameraOffsetY.transform.position = hit.point + hit.normal * 0.1f;
                return;
            }
            else
            {
                CameraOffsetY.transform.position = gameObject.transform.position + mainCamera.transform.forward * -distance;
            }
        }
        else
        {
            CameraOffsetY.transform.position = gameObject.transform.position + mainCamera.transform.forward * -distance;
        }
        CameraOffsetY.transform.position += new Vector3(0f, 2f, 0f);
    }

}
