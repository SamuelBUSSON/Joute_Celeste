using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class ObjectLauncher : MonoBehaviour
{
    public GameObject objectLaunched;
    public float launchStrength = 3.0f;
    public float coolDown = 3.0f;

    private float coolDownTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;

        if (coolDownTimer >= coolDown)
        {
            GameObject objectToLaunch = Instantiate(objectLaunched, transform.position, Quaternion.identity);

            objectToLaunch.gameObject.layer = 10;

            coolDownTimer = 0.0f;
            Vector3 heading = -transform.right;
            objectToLaunch.GetComponent<Rigidbody2D>().velocity = heading * launchStrength;

            Destroy(objectToLaunch.gameObject, 10.0f);

            objectToLaunch.GetComponentInChildren<VisualEffect>().enabled = true;
            objectToLaunch.GetComponent<Projectile>().isLaunched = true;
            
        }
    }
}
