using System.Collections;
using UnityEngine;

public class DecalCollider : MonoBehaviour
{

    [SerializeField] private Material decalMaterial;

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

        audioSource = GameObject.Find("ApplicazioneAdesivo").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision other)
    {
        //Se ho colpito l'aereo
        //&& GameObject.FindAnyObjectByType<ColorManager>().experienceDone
        if(other.gameObject.tag.Equals("PlanePart") && ColorManager.colorManager.experienceDone)
        {
        
            ColorManager.colorManager.ChangeDecalMaterial(decalMaterial);

            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            transform.position = startingPosition;

            audioSource.Play();
        }
    }
}
