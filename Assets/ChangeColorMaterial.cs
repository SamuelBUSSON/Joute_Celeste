using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<TrailRenderer>().material.color = GetComponentInParent<Displacement>().color;
    }    
}
