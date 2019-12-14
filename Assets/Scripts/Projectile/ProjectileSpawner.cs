using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile[] prefabsToSpawn;
    
    public float spawnRadius;
    
    [Header("Propriétés des projectiles lents")]
    [Range(0, 1f)]
    public float probability;

    public float cooldown;

    public int minSpawned;
    public int maxSpawned;

    public int spawnedThreshold;
    
    [Tooltip("Vitesse des projectiles lents")]
    public float slowSpeed;
    
    [Header("Propriétés des projectiles rapides")]
    [Range(0f, 1f)]
    public float fastSpeedProbability;

    [Tooltip("Seconde écoulées pour que les rapides spawn")]
    public float secondToHighSpeed;
    [Tooltip("Vitesse des projectiles rapides")]
    public float fastSpeed;
    
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnProjectile());
    }

    private void Update()
    {
        secondToHighSpeed -= Time.deltaTime;
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
        float speed = slowSpeed;
        if (secondToHighSpeed <= 0.0f && Random.value <= fastSpeedProbability)
        {
            speed = fastSpeed;
        }

        int amount = Random.Range(minSpawned, maxSpawned);
        
        Vector3 startingPosition = GetRandomCameraWorldPoint();
        startingPosition.z = 0f;
        
        Vector3 dir = cam.ViewportToWorldPoint(new Vector3( Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 0));
        dir.z = 0f;
        
        startingPosition += (startingPosition - dir).normalized * spawnRadius;
        startingPosition.z = 0f;
        
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = Random.insideUnitCircle * spawnRadius;
            position += startingPosition;

            var prefabToSpawn = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
            
            var proj = Instantiate(prefabToSpawn, position
                , Quaternion.identity,
                transform);
            //proj.transform.LookAt(dir);
            proj.transform.Rotate(0, 0, Random.Range(-360f, 360.0f));
            proj.GetComponent<Rigidbody2D>().AddForce(-(position - dir).normalized * speed);
        }
    }

    private Vector3 GetRandomCameraWorldPoint()
    {
        int corner = Random.Range(0, 4);

        switch (corner)
        {
            case 0:
                return cam.ViewportToWorldPoint(Vector3.Lerp(Vector3.zero, Vector3.up, Random.Range(0f,1f)));                

            case 1:
                return cam.ViewportToWorldPoint(Vector3.Lerp(Vector3.up, Vector3.one, Random.Range(0f, 1f)));

            case 2:
                return cam.ViewportToWorldPoint(Vector3.Lerp(Vector3.one, Vector3.right, Random.Range(0f, 1f)));

            case 3:
                return cam.ViewportToWorldPoint(Vector3.Lerp(Vector3.zero, Vector3.right, Random.Range(0f, 1f)));

        }
        
        return Vector3.zero;
    }
}
