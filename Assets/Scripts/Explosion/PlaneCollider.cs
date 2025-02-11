using Unity.VisualScripting;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{

    private bool exploded = false;




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
        
        if(collider.tag.Equals("Collision") && !exploded)
        {
            GetComponent<ExplosionManager>().Explode();
            GameObject.FindAnyObjectByType<SceneTransitioner2>().StartTransition();
            GameObject.Find("Esplosione").GetComponent<AudioSource>().Play();
            exploded = true;
        }
        

        
    }


}
