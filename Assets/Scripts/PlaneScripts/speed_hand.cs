using UnityEngine;

public class speed_hand : MonoBehaviour
{

    public GameObject plane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlanePhyRB planePhy2 = plane.GetComponent<PlanePhyRB>();

        Vector3 currentRotation = transform.localEulerAngles;

        currentRotation.z = (planePhy2.vel.magnitude/115.0f*180.0f* 1.94384f) -90.0f;

        //Debug.Log(planePhy2.getSpeed());

        transform.localEulerAngles = currentRotation;
    }
}
