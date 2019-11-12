using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displacement : MonoBehaviour
{

    public float speed = 0.1f;

    private float h;
    private float v;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if (Input.GetAxis("Horizontal") != 0)
        {
            h = Input.GetAxis("Horizontal");
        }
        else
        {
            h = 0;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            v = Input.GetAxis("Vertical");
        }
        else
        {
            v = 0;
        }

        Vector3 displaceVector = new Vector3(h, v, 0).normalized;

        transform.position += displaceVector * speed;

    }
}
