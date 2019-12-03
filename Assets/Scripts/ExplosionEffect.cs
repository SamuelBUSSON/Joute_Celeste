using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    public float speed = 1.1f;
    public float scaleSizeDestroy = 100.0f; 

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= speed;
        if(transform.localScale.x >= scaleSizeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
