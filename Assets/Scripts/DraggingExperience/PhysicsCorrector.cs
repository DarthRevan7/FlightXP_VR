using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCorrector : MonoBehaviour
{

    [SerializeField] private Vector3 startingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider collider)
    {

        if(collider.gameObject.name.Equals("BoundsCollider"))
        {
            transform.position = startingPosition;
        }


    }

}
