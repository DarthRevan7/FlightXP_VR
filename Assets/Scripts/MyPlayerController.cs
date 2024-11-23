using UnityEngine;

public class MyPlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
