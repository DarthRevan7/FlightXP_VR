using UnityEngine;
using TMPro;


//Usare su Empty Gameobject, per avere la posizione in cui una parte di aereo deve attaccarsi
//al modello 3D.


//Si dovrebbe chiamare MeshPlaceholder come classe.


public class DragPoint : MonoBehaviour
{

    //[SerializeField] private DraggableObject draggableObject;
    [SerializeField] private float posDelta = 5f;

    [SerializeField] private TMP_Text textArea;
    [SerializeField] private Material originalMaterial;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision other) {
        string otherName, thisName;
        otherName = other.gameObject.name;
        thisName = gameObject.name;

        textArea.text = "Colliso con: " + other.gameObject.name;

        if(thisName.Equals(otherName))
        {
            GetComponent<MeshRenderer>().material = originalMaterial;
            other.gameObject.SetActive(false);
        }

        
    }
}
