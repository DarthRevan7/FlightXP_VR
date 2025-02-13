using UnityEngine;

public class throttle_hand : MonoBehaviour
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

        Vector3 currentRotation = transform.localEulerAngles;

        currentRotation.z = (planePhy2.getThrottle() * 360.0f) - 90.0f;

        //Debug.Log(planePhy2.getSpeed());

        transform.localEulerAngles = currentRotation;
    }
}
