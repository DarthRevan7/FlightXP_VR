using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public GameObject plane;
    private PlanePhy2 fisicaAereo;
    
    public AudioSource autopilotOn;
    public AudioSource autopilotOff;
    [SerializeField] AudioSource throttle, danger;
    [SerializeField] private TerrainCollisionDetector terrainCollisionDetector;


    // [SerializeField] float volumeControl = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AutopilotChanged.AddListener(CheckAutopilotStatus());
        fisicaAereo = plane.GetComponent<PlanePhy2>();
        throttle.loop = true;
        throttle.Play();
        terrainCollisionDetector = plane.GetComponent<TerrainCollisionDetector>();
        danger.loop = true;
    }

    // Update is called once per frame
    void Update()
    {       
        throttle.volume = fisicaAereo.getThrottle()/4.0f+0.25f;

        if(terrainCollisionDetector.danger && !danger.isPlaying)
        {
            danger.Play();
        }
        if(!terrainCollisionDetector.danger && danger.isPlaying)
        {
            danger.Stop();
        }
    }

    public void PlayAutopilot(bool onoff)
    {
        if(onoff)
            autopilotOn.Play();
        else 
            autopilotOff.Play();
    }
}
