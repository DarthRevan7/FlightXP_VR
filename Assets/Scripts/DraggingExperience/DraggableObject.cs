using UnityEngine;


/*

Assegnare all'oggetto da trascinare sul bordo dell'aereo.

A pensarci meglio dovrei reralizzare dei placeholders con DragPoint e poi semplicemente disabilitarli
anziché cambiare materiale a sta roba qui...

*/

public class DraggableObject : MonoBehaviour
{

    [SerializeField] private string partName;
    [SerializeField] private Material originalMaterial, highlightMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalMaterial = GetComponent<MeshRenderer>().material;
        if(highlightMaterial != null)
        {
            GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetPartName()
    {
        return this.partName;
    }

    public void ChangeMaterial(int material)
    {
        if(material == 0)
        {
            GetComponent<MeshRenderer>().material = originalMaterial;
        }
        else if(material == 1)
        {
            GetComponent<MeshRenderer>().material = highlightMaterial;
        }
        else
        {
            Debug.Log("Wrong assignment to ChangeMaterial method!\n");
        }
    }

}
