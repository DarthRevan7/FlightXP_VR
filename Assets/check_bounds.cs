using UnityEngine;

public class check_bounds : MonoBehaviour
{
    public GameObject plane;

    public Vector2 bound_min;
    public Vector2 bound_max;

    public bool out_of_bounds = false;
    public float out_of_bounds_time = 0;

    private float time_out_of_bounds = 0;

    void Update()
    {
        PlanePhy2 planePhy2 = plane.GetComponent<PlanePhy2>();
        Vector3 pos = planePhy2.getWorldPosition();

        Debug.Log(pos);

        // Check if the plane is out of bounds (assuming the x and z coordinates are used for bounds)
        bool isOutOfBounds = pos.x < bound_min.x || pos.x > bound_max.x || pos.z < bound_min.y || pos.z > bound_max.y;

        if (isOutOfBounds)
        {
            if (!out_of_bounds) // If plane just went out of bounds
            {
                out_of_bounds = true;
                time_out_of_bounds = Time.time; // Start counting time
            }
            out_of_bounds_time = Time.time - time_out_of_bounds; // Update time since it went out of bounds
        }
        else
        {
            out_of_bounds = false; // Plane is back within bounds
            out_of_bounds_time = 0; // Reset time
        }
    }
}
