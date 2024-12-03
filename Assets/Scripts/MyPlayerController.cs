using UnityEngine;

public class MyPlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 5f, rotationSpeed = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float rotation = 0f;

        //GetComponent<Rigidbody>().AddForce(new Vector3(x,0,y).normalized * speed * Time.deltaTime);
        transform.Translate(new Vector3(x,0,y).normalized * speed * Time.deltaTime);
        if(Input.GetKey(KeyCode.E))
        {
            rotation = 1;
            transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            rotation = -1;
            transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);
        }



    }
}
