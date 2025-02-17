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
        PlanePhyRB planePhy2 = plane.GetComponent<PlanePhyRB>();

        Vector3 plane_att = planePhy2.rot;

        float yawDegrees = plane_att.y;

        transform.localRotation = start_rot * Quaternion.Euler(yawDegrees, 0, 0);  
    }
}
