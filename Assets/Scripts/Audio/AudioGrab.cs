using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AudioGrab : MonoBehaviour
{

    [SerializeField] private AudioSource grabAudio;


    public void OnGrab(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs action)
    {
        grabAudio.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        grabAudio = GameObject.Find("PresaStrumento").GetComponent<AudioSource>();

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
