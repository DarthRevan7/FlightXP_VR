using System;
using UnityEngine;
using UnityEngine.UIElements;

public class heading_moving : MonoBehaviour
{

    public GameObject plane;

    Quaternion start_rot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start_rot = transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlanePhy2 planePhy2 = plane.GetComponent<PlanePhy2>();

        Vector3 plane_att = planePhy2.getAttitude();

        float yawDegrees = plane_att.z * Mathf.Rad2Deg;

        transform.localRotation = start_rot * Quaternion.Euler(yawDegrees, 0, 0);  
    }
}
