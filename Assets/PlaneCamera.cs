using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneCamera : MonoBehaviour
{
    [SerializeField]
    private InputActionReference camera_input;

    [SerializeField]
    private float sensitivity = 1f; // Mouse sensitivity

    private float xRotation = 0f;

    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = 0;
        yRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera_input != null)
        {
            Vector2 mouseDelta = camera_input.action.ReadValue<Vector2>();

            // Apply sensitivity and adjust for frame rate
            float mouseX = mouseDelta.x * sensitivity;
            float mouseY = mouseDelta.y * sensitivity;

            if (Mathf.Abs(mouseX) > 180.0 / 4.0 || Mathf.Abs(mouseY) > 180.0 / 4.0) return;

            // Adjust vertical rotation (clamp to prevent flipping)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation += mouseX;

            

            // Apply rotations
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // Vertical rotation (pitch)
        }
    }
}
