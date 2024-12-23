using UnityEngine;


//Usare su Empty Gameobject, per avere la posizione in cui una parte di aereo deve attaccarsi
//al modello 3D.


//Si dovrebbe chiamare PlaceHolder come classe.


public class DragPoint : MonoBehaviour
{

    [SerializeField] private DraggableObject draggableObject;
    [SerializeField] private float posDelta = 5f;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckPosition()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, posDelta);

        foreach(Collider c in colliders)
        {
            
        }

        return false;
    }
}
