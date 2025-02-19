using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private LayerMask hitLayers;
    private LayerMask ignoreLayers;
    private float lifetime;

    public GameObject explPrefab;

    public void Initialize(Vector3 velocity, LayerMask layers, float lifeTime, LayerMask ignoreL)
    {
        rb = GetComponent<Rigidbody>();
        hitLayers = layers;
        ignoreLayers = ignoreL;
        lifetime = lifeTime;

        if (rb != null)
        {
            //Debug.Log($"Setting vel to: {velocity}");
            rb.linearVelocity = velocity;
        }
        else
        {
            Debug.LogError($"Failed to get bullet rigid body");
        }

        // Destroy the bullet after some time
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((ignoreLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Ignoring hit: " + other.gameObject.name);
            return;
        }
        if ((hitLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Bullet hit: " + other.gameObject.name);
            
            GameObject expl = Instantiate(explPrefab, rb.position, Quaternion.identity);

            Destroy(other.gameObject);
            Destroy(gameObject); // Destroy bullet on impact
        }
        else
        {
            Debug.Log("Hit non-target: " + other.gameObject.name);
            Destroy(gameObject); // Destroy bullet on impact
        }
    }
}

