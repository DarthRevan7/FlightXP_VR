using UnityEngine;

public class TerrainCollisionDetector : MonoBehaviour
{
    public GameObject plane;
    public float checkDistance = 100f; // Adjust based on object size
    public float collisionDistance = 1.0f;
    public float dangerDistance = 50.0f;
    public Vector3[] directions = {
        Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    };

    public bool danger = false;

    private bool called = false;    

    void Update()
    {
        danger = false;
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, checkDistance))
            {
                //Debug.Log(hit);
                if (hit.collider.GetComponent<Terrain>())
                {
                    //Debug.Log($"Collided with terrain! {hit.distance} in direction {direction}");
                    if (hit.distance < dangerDistance) danger = true;

                    if (hit.distance < collisionDistance && !called)
                    {
                        //Debug.Log("Collision!!");
                        GetComponent<ExplosionManager>().Explode();
                        called = true;
                    }
                }
            }
        }
    }
}
