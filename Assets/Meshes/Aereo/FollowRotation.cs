using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class FollowRotation : MonoBehaviour
{
    public GameObject other;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = other.transform.localRotation;
    }
}
