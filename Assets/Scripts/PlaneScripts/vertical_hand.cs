using UnityEngine;

public class vertical_hand : MonoBehaviour
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

        float hand_rot = planePhy2.vel.y / 20.0f * 180.0f;

        if (hand_rot > 170.0f) hand_rot = 170.0f;
        if (hand_rot < -170.0f) hand_rot = -170.0f;

        currentRotation.z = (hand_rot) - 180.0f;

        //Debug.Log(planePhy2.getSpeed());

        transform.localEulerAngles = currentRotation;
    }
}
