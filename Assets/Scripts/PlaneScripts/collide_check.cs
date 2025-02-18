using UnityEngine;

public class TerrainCollisionDetector : MonoBehaviour
{
    public GameObject plane;
    public float dangerDistance = 90.0f;
    // public Vector3[] directions = {
    //     Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    // };

    public bool danger = false;

    // private bool called = false;    

    public LayerMask checkLayers;

    void Update()
    {
        danger = Physics.CheckSphere(plane.GetComponent<PlanePhyRB>().pos, dangerDistance, checkLayers);

        // foreach (Vector3 direction in directions)
        // {
        //     if (Physics.Raycast(transform.position, direction, out RaycastHit hit, checkDistance))
        //     {
        //         //Debug.Log(hit);
        //         if (hit.collider.GetComponent<Terrain>())
        //         {
        //             //Debug.Log($"Collided with terrain! {hit.distance} in direction {direction}");
        //             if (hit.distance < dangerDistance) danger = true;

        //             if (hit.distance < collisionDistance && !called)
        //             {
        //                 //Debug.Log("Collision!!");
                        
        //             }
        //         }
        //     }
        // }
    }
}
