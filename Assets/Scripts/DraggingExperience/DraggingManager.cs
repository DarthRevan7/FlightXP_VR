using UnityEngine;

public class DraggingManager : MonoBehaviour
{

    public static DraggingManager instance;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
