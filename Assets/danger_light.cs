using UnityEngine;

public class EmissionBlink : MonoBehaviour
{
    public Renderer targetRenderer;  // Assign the object with the emissive material
    public Color emissionColor = Color.red;
    public float blinkSpeed = 2f;

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
        TerrainCollisionDetector coll = plane.GetComponent<TerrainCollisionDetector>();

        if (coll.danger)
        {
            float intensity = Mathf.PingPong(Time.time * blinkSpeed, 1f); // Oscillate between 0 and 1
            intensity = intensity * intensity;
            Color finalColor = baseColor * intensity;  // Scale the emission color
            mat.SetColor("_EmissionColor", finalColor);
        }
        else mat.SetColor("_EmissionColor", Color.black);

    }
}

