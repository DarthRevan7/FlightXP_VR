using Unity.VisualScripting;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag.Equals("Collision"))
        {
            GetComponent<ExplosionManager>().Explode();
            GameObject.FindAnyObjectByType<SceneTransitioner>().StartTransition();
        }
    }
    */


    void OnTriggerEnter(Collider collider)
    {
        
        if(collider.tag.Equals("Collision"))
        {
            GetComponent<ExplosionManager>().Explode();
            GameObject.FindAnyObjectByType<SceneTransitioner>().StartTransition();
        }
        

        
    }


}
