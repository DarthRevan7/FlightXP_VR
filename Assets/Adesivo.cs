using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Adesivo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject sticker = new GameObject();

    private Vector3 origineRay = Camera.main.transform.position;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Fire1"))
        {
            PiazzaAdesivo();
        }
    }

    public void PiazzaAdesivo()
    {
       
        Ray ray = Camera.main.ScreenPointToRay(origineRay);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray,out hitInfo, 100f))
        {
            Instantiate(sticker, hitInfo.point, Quaternion.FromToRotation(Vector3.up,hitInfo.normal));
        }
    }
}
