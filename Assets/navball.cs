using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class navball : MonoBehaviour
{

    [SerializeField]
    Transform ship;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //var forw = ship.forward;
        //forw.y = 0f;
        //if (forw.sqrMagnitude > 0.0001f) forw.Normalize();

        //var yaw = -Vector3.SignedAngle(forw, Vector3.forward, Vector3.up);
        //var pitch = Vector3.SignedAngle(forw, this.ship.forward, this.ship.right);

        //var rot = Quaternion.AngleAxis(pitch, Vector3.right) * Quaternion.AngleAxis(yaw, Vector3.up);
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, 0.5f); //used a little slerp to give a smoother look

        //transform.localRotation = new Quaternion(ship.localRotation.y, ship.localRotation.x, ship.localRotation.z, ship.localRotation.w);
        transform.localRotation = new Quaternion(ship.localRotation.z, -ship.localRotation.x, ship.localRotation.y, ship.localRotation.w);

        //transform.rotation = Quaternion.Euler(-90.0f, 0f, 0f);
    }
}
