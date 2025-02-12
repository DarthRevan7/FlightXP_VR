using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCorrector : MonoBehaviour
{

    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private BoxCollider boundsCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boundsCollider.bounds.Contains(transform.position))
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            //Azzera la velocità dell'oggetto.
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    

}
