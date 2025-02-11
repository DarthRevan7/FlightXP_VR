using UnityEngine;

public class BarrelCollider : MonoBehaviour
{

    [SerializeField] private Material barrelMaterial;

    [SerializeField] private DraggingManager draggingManager;
    [SerializeField] private GameObject particlePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draggingManager = GameObject.FindAnyObjectByType<DraggingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision other)
    {
        //Se ho colpito l'aereo
        if(other.gameObject.tag.Equals("PlanePart"))
        {
            /*
            if(draggingManager.GetExperienceFinished())
            {
                draggingManager.SetPlaneMaterial(barrelMaterial);
            }
            */
            draggingManager.SetPlaneMaterial(barrelMaterial);
        }
    }


}
