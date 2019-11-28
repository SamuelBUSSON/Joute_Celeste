using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile prefabToSpawn;
    
    [Range(0, 1f)]
    public float probability;

    public float cooldown;
    public int maxSpawned;
    public float radius;

    public Color radiusColor;

    private void Start()
    {
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        //TODO: make this stop when the round stops
        while (true)
        {
            if (probability <= Random.value && maxSpawned > transform.childCount)
                Instantiate(prefabToSpawn, transform.position + (Random.insideUnitSphere * radius), Quaternion.identity,
                    transform);
            
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = radiusColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
