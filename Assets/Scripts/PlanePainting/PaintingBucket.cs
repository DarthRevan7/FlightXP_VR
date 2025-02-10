using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PaintingBucket : MonoBehaviour
{

    [SerializeField] private Material material;
    [SerializeField] private Vector3 originalPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("PlanePart"))
        {
            if(DraggingManager.instance.GetExperienceFinished())
            {
                DraggingManager.instance.SetPlaneMaterial(material);
                //Set original position
                transform.position = originalPosition;
            }
        }
    }
}
