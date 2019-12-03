using System.Collections;
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
            {
                Vector2 position = (Random.insideUnitCircle * radius);
                var proj = Instantiate(prefabToSpawn, transform.position + new Vector3(position.x, position.y, 0),
                    Quaternion.identity,
                    transform);
                proj.GetComponent<Rigidbody2D>().velocity = 0.1f * new Vector2(Random.value, Random.value);
            }
                
            
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = radiusColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
