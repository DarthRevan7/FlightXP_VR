using UnityEngine;


/*

Assegnare all'oggetto da trascinare sul bordo dell'aereo.

A pensarci meglio dovrei reralizzare dei placeholders con DragPoint e poi semplicemente disabilitarli
anziché cambiare materiale a sta roba qui...

*/

/*

gameobject.name è il nome assegnato nella Hierarchy.

*/

[RequireComponent(typeof(MeshCollider))]
public class DraggableObject : MonoBehaviour
{

    [SerializeField] private string partName;
    [SerializeField] private Material originalMaterial, highlightMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        partName = gameObject.name;
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetPartName()
    {
        return partName;
    }

    

    

}
