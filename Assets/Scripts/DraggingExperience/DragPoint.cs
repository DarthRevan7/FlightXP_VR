using UnityEngine;
using TMPro;



//Si dovrebbe chiamare MeshPlaceholder come classe.

//Da applicare alla mesh "placeholder".
//I nomi dei children sotto all'oggetto container devono essere uguali x entrambe le mesh.



public class DragPoint : MonoBehaviour
{
    //Area di testo x debug
    [SerializeField] private TMP_Text textArea;
    //Material originale dell'oggetto, non highlight
    [SerializeField] private Material originalMaterial;

    private void OnCollisionEnter(Collision other) {
        //Ricavo le stringhe dei nomi dei gameobjects
        string otherName, thisName;
        otherName = other.gameObject.name;
        thisName = gameObject.name;

        //Riga x debug, si può eliminare o commentare
        textArea.text = "Colliso con: " + other.gameObject.name;

        //Se il nome degli oggetti che sto confrontando corrisponde, allora cambio il materiale
        //dell'oggetto con script DragPoint e distruggo l'altro oggetto.
        if(thisName.Equals(otherName))
        {
            GetComponent<MeshRenderer>().material = other.gameObject.GetComponent<MeshRenderer>().material;
            Destroy(other.gameObject);
        }

        
    }
}
