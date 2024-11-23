using UnityEngine;

public class MyObstacleScript : MonoBehaviour
{

    [SerializeField] private float float1 = 34f;
    private int intero = 33;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("34___");
        Debug.Log("Intero" + intero.ToString());
        Debug.Log("Float: " + float1.ToString());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
