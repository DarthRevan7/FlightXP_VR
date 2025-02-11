using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public GameObject plane;
    private PlanePhy2 fisicaAereo;
    
    public AudioSource autopilotOn;
    public AudioSource autopilotOff;
    [SerializeField] AudioSource throttle;


    [SerializeField] float volumeControl = 10.0f;

    public UnityEvent AutopilotChanged { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AutopilotChanged.AddListener(CheckAutopilotStatus());
        PlanePhy2 fisicaAereo = plane.GetComponent<PlanePhy2>();
    }

    // Update is called once per frame
    void Update()
    {
       

        fisicaAereo.getThrottle();
        throttle.loop = true;
        throttle.Play();

        if (fisicaAereo.autopilotEngaged)
        {
            autopilotOn.playOnAwake = true;
            autopilotOn.loop = false;
            autopilotOn.Play();
        }
        else
        {

        }
    }

    public bool CheckAutopilotStatus()
    {

        return fisicaAereo.autopilotEngaged;
    }
}
