using System;
using UnityEngine;

public class recenter : MonoBehaviour
{
    public GameObject plane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlanePhy2 planePhy2 = plane.GetComponent<PlanePhy2>();
        Vector3 pos = planePhy2.transform.position;

        if(pos.magnitude > 200)
        {
            plane.transform.position = Vector3.zero;
            planePhy2.increaseOffset(-pos);
            transform.position -= pos;
        }
    }
}
