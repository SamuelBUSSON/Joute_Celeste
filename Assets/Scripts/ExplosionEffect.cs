using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    public bool mini;

    public float speed = 1.1f;
    public float scaleSizeDestroy = 100.0f;

    private Vector2 scale;

    private void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        scale.x += mini ? -speed : speed;
        scale.y += mini ? -speed : speed;

        transform.localScale = scale;
        if(transform.localScale.x >= scaleSizeDestroy)
        {
            Destroy(gameObject);
        }
        if(transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
