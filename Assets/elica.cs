using UnityEngine;

public class elica : MonoBehaviour
{

    public GameObject plane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlanePhy2 plane_phy = plane.GetComponent<PlanePhy2>();

        float vel = plane_phy.getSpeed();

        transform.Rotate(Vector3.left, 60*vel * Time.deltaTime);
    }
}
