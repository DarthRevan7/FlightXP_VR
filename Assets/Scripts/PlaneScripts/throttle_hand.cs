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
        PlanePhyRB planePhy2 = plane.GetComponent<PlanePhyRB>();

        Vector3 currentRotation = transform.localEulerAngles;

        currentRotation.z = (planePhy2.current_throttle_value * 360.0f) - 90.0f;

        //Debug.Log(planePhy2.getSpeed());

        transform.localEulerAngles = currentRotation;
    }
}
