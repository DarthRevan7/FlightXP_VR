using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    public float desiredFPS = 60f;

    [Range(0f, 5000f)]
    public float RPM = 0f;
    public bool reversed = false;
    [SerializeField] private float angle = 0f;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float singleFrameRotation = RPM / 60f * 360f / desiredFPS;
        singleFrameRotation = RoundRotation(singleFrameRotation);

        angle += singleFrameRotation * desiredFPS * Time.deltaTime;
        angle %= 360f;

        if (reversed)
        {
            transform.localRotation = initialRotation * Quaternion.AngleAxis(-angle, Vector3.forward);
        } else
        {
            transform.localRotation = initialRotation * Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        // Mix motion blur textures
        float blurStage = Mathf.Min(RPM / 4000f * 4f, 4f);

        GetComponent<Renderer>().material.SetFloat(
            "_Blade_0_Weight", Mathf.Clamp01(1f - blurStage)
        );
        Vector4 otherWeights = new Vector4(0f, 0f, 0f, 0f)
        {
            x = TextureWeight(blurStage, 1f),
            y = TextureWeight(blurStage, 2f),
            z = TextureWeight(blurStage, 3f),
            w = TextureWeight(blurStage, 4f)
        };

        GetComponent<Renderer>().material.SetVector("_Weights", otherWeights);
    }

    float RoundRotation(float angle)
    {
        // Round to nearest multiple of 180
        return angle - (int)((angle + 90f) / 180f) * 180f;
    }

    float TextureWeight(float stage, float textureIndex)
    {
        return Mathf.Clamp01(1f - Mathf.Abs(stage - textureIndex));
    }
}
