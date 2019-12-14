using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionEffect : MonoBehaviour
{

    public bool mini;

    public float speed = 1.1f;
    public float scaleSizeDestroy = 100.0f;

    public GameObject objectAfter;
    public float scaleToInstantiate = 20f;

    private Vector2 scale;
    private Material material;

    private void Start()
    {
        scale = transform.localScale;

        material = GetComponent<SpriteRenderer>().material;
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

        material.DOFloat(1.0f, "_DistorsionStrenth", 0.25f);

        if (scale.x >= scaleToInstantiate)
        {
            if (objectAfter)
            {
                Instantiate(objectAfter, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
                objectAfter = null;
            }
        }
    }
}
