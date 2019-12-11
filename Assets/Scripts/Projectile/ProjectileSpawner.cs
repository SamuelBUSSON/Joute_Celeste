using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile prefabToSpawn;
    
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
        
        startingPosition += (startingPosition - dir).normalized * 1.25f;
        startingPosition.z = 0f;
        
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = Random.insideUnitCircle;
            position += startingPosition;
            
            var proj = Instantiate(prefabToSpawn, position
                , Quaternion.identity,
                transform);
            //proj.transform.LookAt(dir);
            proj.transform.Rotate(0, 0, Random.Range(-5f, 5f));
            proj.GetComponent<Rigidbody2D>().AddForce(-(position - dir).normalized * speed);
        }
    }

    private Vector3 GetRandomCameraWorldPoint()
    {
        int corner = Random.Range(0, 6);

        switch (corner)
        {
            case 0: 
                return cam.ViewportToWorldPoint(Vector3.zero);

            case 1: 
                return cam.ViewportToWorldPoint(Vector3.up);

            case 2:
                return cam.ViewportToWorldPoint(Vector3.one);

            case 3:
                return cam.ViewportToWorldPoint(Vector3.right);

            case 4:
                return cam.ViewportToWorldPoint(Vector3.right/2);

            case 5:
                return cam.ViewportToWorldPoint(Vector3.up / 2);

        }
        
        return Vector3.zero;
    }
}
