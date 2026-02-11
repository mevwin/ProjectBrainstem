using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public Transform CameraOffsetX;
    public Transform CameraOffsetY;
    public GameObject mainCamera;
    public float lookSpeed = 1;

    InputAction look;
    Vector2 lookAmt;

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
    }

    private void FixedUpdate()
    {
        Rotating();
    }

    private void Rotating()
    {
        if (lookAmt.x != 0)
        {
            gameObject.transform.Rotate(0, lookAmt.x * lookSpeed, 0);
        }

        if (lookAmt.y != 0)
        {
            if (Mathf.Abs(lookAmt.y) > 1)
            {
                lookAmt.y /= Mathf.Abs(lookAmt.y);
            }

            if (lookAmt.y < 0)
            {
                if (CameraOffsetY.localEulerAngles.x > 0 && CameraOffsetY.localEulerAngles.x < 350)
                {
                    CameraOffsetY.Rotate(lookAmt.y * lookSpeed, 0, 0, Space.Self);
                }
            }
            else
            {
                if (CameraOffsetY.localEulerAngles.x < 24|| CameraOffsetY.localEulerAngles.x > 350)
                {
                    CameraOffsetY.Rotate(lookAmt.y * lookSpeed, 0, 0, Space.Self);
                }
            }
        }
    }

    private void LateUpdate()
    {
        // mainCamera.transform.LookAt(transform.position);
        // mainCamera.transform.Rotate(0, 0, -transform.rotation.z, Space.Self);
    }
}
