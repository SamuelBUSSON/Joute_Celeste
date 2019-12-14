using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{

    public GameObject starObject;
    public GameObject blackHole;

    public float timerToSpawnStar;
    public float timeBetweenSpawnStar = 5.0f;
    public float timerToSpawnBlackHole = 10.0f;


    public float speed = 500.0f;
    public float blackHoleSpeed = 1000f;

    private float timer;
    private float blackHoleTimer;
    private float timerTimeBetweenSpawnStar;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        timerTimeBetweenSpawnStar += Time.deltaTime;

        blackHoleTimer += Time.deltaTime;

        if(blackHoleTimer >= timerToSpawnBlackHole)
        {
            SpawnObject(blackHole, true);
            timerToSpawnBlackHole = float.PositiveInfinity;
        }

        if (timer >= timerToSpawnStar)
        {
            if(timerTimeBetweenSpawnStar >= timeBetweenSpawnStar)
            {
                SpawnObject(starObject);

                timerTimeBetweenSpawnStar = 0.0f;
            }
        }
    }

    private void SpawnObject(GameObject objectToSpawn, bool isBlackHole = false)
    {
        Vector3 startingPosition = GetRandomCameraWorldPoint();
        startingPosition.z = 0f;

        Vector3 dir = cam.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 0));
        dir.z = 0f;

        startingPosition += (startingPosition - dir).normalized * 1.25f;
        startingPosition.z = 0f;

        Vector3 position = Random.insideUnitCircle;
        position += startingPosition;

        var proj = Instantiate(objectToSpawn, position
            , Quaternion.identity,
            transform);

        proj.transform.Rotate(0, 0, Random.Range(-5f, 5f));
        proj.GetComponent<Rigidbody2D>().AddForce(-(position - dir).normalized * (isBlackHole ? blackHoleSpeed : speed));
    }

    private Vector3 GetRandomCameraWorldPoint()
    {
        int corner = Random.Range(0, 4);

        switch (corner)
        {
            case 0:
                return cam.ViewportToWorldPoint(Vector3.Lerp(Vector3.zero, Vector3.up, Random.Range(0f, 1f)));

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
