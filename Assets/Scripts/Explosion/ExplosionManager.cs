using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{

    [SerializeField] private List<ParticleSystem> explosions;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        foreach(ParticleSystem ps in explosions)
        {
            ps.Play();
        }
    }
}
