using Unity.VisualScripting;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{

    public TargetManager tmanager;
    public PlanePhyRB plane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tmanager.targetPositions.Count == 0) gameObject.SetActive(false);
        else
        {
            Rigidbody rb = plane.GetComponent<Rigidbody>();

            Vector3 closest = tmanager.targetPositions[0];
            float best_dist = (tmanager.targetPositions[0]-rb.position).magnitude;
            for(int i = 1; i < tmanager.targetPositions.Count; i++)
            {
                float ndist = (tmanager.targetPositions[i]-rb.position).magnitude;
                if(ndist < best_dist) 
                {
                    best_dist = ndist;
                    closest = tmanager.targetPositions[1];
                }
            }

            gameObject.SetActive(true);
            
            Quaternion rot = Quaternion.Euler( 0, rb.rotation.eulerAngles.x, 0 );

            Vector3 dir = closest - rb.position;

            dir = rot * dir;

            Vector2 dir2 = new Vector2(dir.x, dir.z);
            dir2 = dir2.normalized;

            transform.localPosition = new Vector3(0, dir2.y, dir2.x) * 0.035f;
        }

        
    }

    
}
