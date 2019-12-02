using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalTexture("_Particle_Distort", GetComponent<Camera>().activeTexture);
        Shader.SetGlobalFloat("_Float_Test", 0.0f);       
    }
}
