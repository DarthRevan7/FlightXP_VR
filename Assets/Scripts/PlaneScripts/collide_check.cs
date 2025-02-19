using System;
using UnityEngine;

public class TerrainCollisionDetector : MonoBehaviour
{
    public GameObject plane;
    public float dangerDistance = 90.0f;
    public float dangerMinVel = 50.0f;
    // public Vector3[] directions = {
    //     Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    // };

    public bool danger = false;

    // private bool called = false;   
    bool exploded = false; 

    public LayerMask checkLayers;

    public float collisionTreshold = 10000.0f;

    public String death_scene_name;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude > collisionTreshold)
        {
            Debug.Log($"Collided with: {collision.gameObject.name}, with impulse: {collision.impulse}");

            Explode();
        }
    }

    void Update()
    {
        danger = Physics.CheckSphere(plane.GetComponent<PlanePhyRB>().pos, dangerDistance, checkLayers)
            && dangerMinVel <= plane.GetComponent<PlanePhyRB>().vel.magnitude;
    }

    void Explode()
    {
        if(!exploded)
        {
            exploded = true;
            FindAnyObjectByType<ExplosionManager>().Explode();
            SceneTransitioner2 tr = FindAnyObjectByType<SceneTransitioner2>();
            tr.sceneName = death_scene_name;
            tr.StartTransition();
        }
    }
}
