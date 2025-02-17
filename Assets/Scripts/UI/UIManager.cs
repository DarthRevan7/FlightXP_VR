using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public Camera camera;

    public bool vr=false;

    public int h, w;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResolution();
        camera.transform.position = new Vector3((float)w/2.0f, (float)h/2.0f, camera.transform.position.z);
        // camera.orthographicSize = (float)h/2.0f;
        RectTransform rect = canvas.GetComponent<RectTransform>();
        rect.position = new Vector3((float)w/2.0f, (float)h/2.0f, 0f);
        rect.sizeDelta = new Vector2(w,h);


    }

    void UpdateResolution()
    {
        if(vr)
        {
            w = XRSettings.eyeTextureWidth;
            h = XRSettings.eyeTextureHeight;
        }
        else
        {
            w = Screen.width;
            h = Screen.height;
        }
    }
}
