using System.Collections;
using UnityEngine;

public class BarrelCollider : MonoBehaviour
{

    [SerializeField] private Material barrelMaterial;

    [SerializeField] private DraggingManager draggingManager;
    [SerializeField] private GameObject particlePrefab;

    [SerializeField] private Color splashColor;
    [SerializeField] private float timeToDestruction = 2f;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draggingManager = GameObject.FindAnyObjectByType<DraggingManager>();
        startingPosition = transform.position;

        audioSource = GameObject.Find("ApplicazioneVernice").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision other)
    {
        //Se ho colpito l'aereo
        if(other.gameObject.tag.Equals("PlanePart") && draggingManager.GetExperienceFinished())
        {
            /*
            if(draggingManager.GetExperienceFinished())
            {
                draggingManager.SetPlaneMaterial(barrelMaterial);
            }
            */
            draggingManager.SetPlaneMaterial(barrelMaterial);

            GameObject splash = GameObject.Instantiate(particlePrefab, other.contacts[0].point, Quaternion.identity);
            splash.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.color = splashColor;
            splash.GetComponentsInChildren<ParticleSystem>()[1].GetComponent<Renderer>().material.color = splashColor;
            splash.GetComponentsInChildren<ParticleSystem>()[0].GetComponent<Renderer>().material.color = splashColor;

            GameObject.Destroy(splash, timeToDestruction);

            transform.position = startingPosition;

            audioSource.Play();
        }
    }

    


}
