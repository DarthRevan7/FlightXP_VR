using UnityEngine;

public class elica_effect : MonoBehaviour
{
    public GameObject plane;

    [Range(0f, 200.0f)]
    public float propellerSpeed = 0.0f;

    public float mesh_cutoff_speed = 150.0f;

    public float planes_cutoff_speed = 50.0f;

    public GameObject elica;
    public GameObject plane1;
    public GameObject plane2;

    private Renderer rend1, rend2, rend3;

    public float elica_t = 1.0f;
    public float plane1_t = 1.0f;
    public float plane2_t = 1.0f;

    void Start()
    {
        rend1 = elica.GetComponent<Renderer>();
        rend2 = plane1.GetComponent<Renderer>();
        rend3 = plane2.GetComponent<Renderer>();
    }

    void Update()
    {
        PlanePhyRB planePhyRB = plane.GetComponent<PlanePhyRB>();
        propellerSpeed = planePhyRB.current_throttle_value*250.0f;

        elica_t = 1.0f - Mathf.SmoothStep(0.0f, 1.0f, (propellerSpeed - planes_cutoff_speed) / (mesh_cutoff_speed - planes_cutoff_speed));
        elica.transform.Rotate(Vector3.right * propellerSpeed * 360.0f / 60.0f * Time.deltaTime);

        plane1_t = Mathf.SmoothStep(0.0f, 1.0f, (propellerSpeed-planes_cutoff_speed) / (mesh_cutoff_speed-planes_cutoff_speed)*2.0f);
        plane2_t = plane1_t;
        UpdateAlpha(); // Continuously update alpha
    }

    private void UpdateAlpha()
    {
        SetAlpha(rend1, elica_t);
        SetAlpha(rend2, plane1_t);
        SetAlpha(rend3, plane2_t);
    }

    private void SetAlpha(Renderer rend, float a)
    {
        if (rend != null)
        {
            foreach (Material mat in rend.materials)
            {
                Color color = mat.color;
                color.a = Mathf.Clamp01(a);
                mat.color = color;
            }
        }
    }
}
