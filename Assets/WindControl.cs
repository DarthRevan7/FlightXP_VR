using UnityEngine;

public class WindControl : MonoBehaviour    
{
    public float windDirection;
    public float strength;
    public Cloth windSock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        windDirection = 0.0f;
        windSock = this.GetComponent<Cloth>();
    }

    // Update is called once per frame
    void Update()
    {
        windSock.externalAcceleration = new Vector3(Mathf.Clamp(strength, 0, 150), 0, 0); ;
        windDirection = Mathf.Clamp(windDirection, -180, 180);
        windSock.GetComponentInParent<Transform>().rotation= Quaternion.Euler(new Vector3(0, windDirection, 0));
        
    }
}
