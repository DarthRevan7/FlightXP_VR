using UnityEngine;

public class AutopilotLight : MonoBehaviour
{
    public Renderer targetRenderer;  // Assign the object with the emissive material
    public Color emissionColor = Color.red;

    private Material mat;
    private Color baseColor;

    public GameObject plane;

    void Start()
    {
        mat = targetRenderer.material;  // Get the material instance
        baseColor = emissionColor; // Store the original color
        mat.EnableKeyword("_EMISSION"); // Ensure emission is active
    }

    void Update()
    {
        InputLimiter input_limiter = plane.GetComponent<InputLimiter>();
        float intensity = 0;
        if (input_limiter.autopilotEngaged)
        {
            intensity = 1.0f;
        }

        Color finalColor = baseColor * intensity;  // Scale the emission color
        mat.SetColor("_EmissionColor", finalColor);
    }
}

