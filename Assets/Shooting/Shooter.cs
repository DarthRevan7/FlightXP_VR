using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign in Inspector
    public float bulletSpeed = 50f;
    public float bulletLifetime = 5f;
    public LayerMask hitLayers; // Set this to detect specific layers
    public LayerMask ignoreLayers;
    public float shotsPerSecond = 1.0f;

    public float shotPosOffset = 2.0f;

    Rigidbody plane_rb;

    public GameObject explPrefab;

    public InputActionReference shoot_command;

    public void Start()
    {
        plane_rb = GetComponentInParent<Rigidbody>();
    }

    public void Shoot()
    {
        if (bulletPrefab == null) return;

        // Spawn the bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position+ plane_rb.transform.forward* shotPosOffset, transform.rotation);

        // Get bullet script and set its velocity
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.explPrefab = explPrefab;
            bulletScript.Initialize(plane_rb.transform.forward * bulletSpeed + plane_rb.linearVelocity, hitLayers, bulletLifetime, ignoreLayers);
        }
    }

    float last_shot = 0;
    
    public void Update()
    {
        if (shoot_command.action.IsPressed() && Time.time - last_shot > 1.0f / shotsPerSecond)
        {
            last_shot = Time.time;
            Shoot();
        }
    }
}

