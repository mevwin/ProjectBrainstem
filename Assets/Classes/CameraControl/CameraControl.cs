using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Transform CameraOffsetY;
    public GameObject mainCamera;
    [SerializeField] private float sensitivity, lowerBound, upperBound;

    InputAction look;
    Vector2 lookAmt;

    float vertRotation;

    void Start()
    {
        // Get InputAction references from Project-wide input actions.
        if (InputSystem.actions)
        {
            look = InputSystem.actions.FindAction("Player/Look");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        lookAmt = look.ReadValue<Vector2>();
        Rotating();
        Positioning();
    }

    private void FixedUpdate()
    {
        //Rotating();
    }

    private void Rotating()
    {
        if (lookAmt.x != 0f)
        {
            lookAmt.x *= sensitivity;

            gameObject.transform.Rotate(0, lookAmt.x, 0);
        }

        if (lookAmt.y != 0f)
        {
            lookAmt.y *= sensitivity;

            vertRotation = Mathf.Clamp(vertRotation - lookAmt.y, lowerBound, upperBound);

            CameraOffsetY.transform.localRotation = Quaternion.Euler(vertRotation, 0, 0);
        }
    }

    private void Positioning()
    {
        if (Physics.Raycast(gameObject.transform.position, mainCamera.transform.forward * -1, out RaycastHit hit))
        {
            if (hit.distance < 6f)
            {
                Debug.Log(hit.transform.gameObject.name);
                CameraOffsetY.transform.position = hit.point + mainCamera.transform.forward;
            }
            else
            {
                CameraOffsetY.transform.position = gameObject.transform.position + mainCamera.transform.forward * -6;
            }
        }
        else
        {
            CameraOffsetY.transform.position = gameObject.transform.position + mainCamera.transform.forward * -6;
        }

    }

}
