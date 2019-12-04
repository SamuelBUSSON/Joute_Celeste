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

    public int minSpawned;
    public int maxSpawned;

    public int spawnedThreshold;

    public float speed;
    
    Vector2 center = new Vector2(Screen.height, Screen.width);

    private void Start()
    {
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        //TODO: make this stop when the round stops
        while (true)
        {
            if (probability <= Random.value && transform.childCount < spawnedThreshold)
            {
                //TODO: add proba to event
                SpawnMeteorRain();
                
            }
                
            
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void SpawnMeteorRain()
    {
        int amount = Random.Range(minSpawned, maxSpawned);

        Vector2 startingPosition = ((Random.insideUnitCircle + Vector2.one) * (Screen.width *.25f));
        
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = startingPosition + (Random.insideUnitCircle * 4f);
            var proj = Instantiate(prefabToSpawn, new Vector3(position.x, position.y, 0),
                Quaternion.identity,
                transform);
            proj.transform.LookAt(center);
            proj.transform.Rotate(0, 0, Random.Range(-5f, 5f));
            proj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
        }
    }

    private void SpawnMeteorBelt()
    {
        int amount = Random.Range(minSpawned, maxSpawned);

        Vector2 startingPosition = ((Random.insideUnitCircle + Vector2.one) * (Screen.width *.25f));

        for (int i = 0; i < amount; i++)
        {
            
        }
    }
}
