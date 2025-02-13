using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;


public class PlaneCamera : MonoBehaviour
{
    [SerializeField]
    private InputActionReference camera_input;
    public bool vr;

    [SerializeField]
    private float sensitivity = 1f; // Mouse sensitivity

    private float xRotation = 0f;

    private float yRotation = 0f;


    private UnityEngine.XR.InputDevice headDevice;

    // Start is called before the first frame update
    void Start()
    {
        

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = 0;
        yRotation = 0;


        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.Head, inputDevices);

        if (inputDevices.Count > 0)
        {
            headDevice = inputDevices[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(vr)
        {
            //Quaternion qin = camera_input.action.ReadValue<Quaternion>();
            //transform.localRotation = qin;

            /* Soluzione 1
            Vector3 headRotation = InputTracking.GetLocalRotation(XRNode.Head).eulerAngles;
            transform.localRotation = Quaternion.Euler(headRotation);
            */


            if (headDevice.isValid)
        {
            // Ottieni la rotazione dell'HMD
            if (headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion headRotation))
            {
                // Applica la rotazione alla camera
                transform.localRotation = headRotation;
            }
        }



        }
        else
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
}
