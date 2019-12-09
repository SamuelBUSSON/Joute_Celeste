using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

    public float speed = 0.5f;

    private MeshRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);

        spriteRenderer.material.SetTextureOffset("_MainText", offset);

    }
}
