using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject starPrefab;

    public float distance = 1.0f;
    public float coolDown = 0.2f;

    public float launchStrength = 2.0f;

    public bool autoAim = false;

    public Transform enemyPos;

    private Transform currentChild;
    private float angle = 0.0f;
    private Vector3 displaceAngleVector;

    private float coolDownTimer = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        displaceAngleVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount != 1)
        {
            currentChild = Instantiate(starPrefab, transform.position, Quaternion.identity).transform;

            currentChild.SetParent(transform);

            currentChild.transform.position = transform.position + displaceAngleVector;
        }
        else
        {

            if (autoAim)
            {

                Vector3 heading = enemyPos.position - transform.position;

                angle = Mathf.Atan2(heading.normalized.x, heading.normalized.y);

                displaceAngleVector.x = distance * Mathf.Sin(angle);
                displaceAngleVector.y = distance * Mathf.Cos(angle);

                currentChild.transform.position = transform.position + displaceAngleVector;
            }
            else
            {
                float x = Input.GetAxis("ControllerVertical");
                float y = Input.GetAxis("ControllerHorizontal");

                if (x != 0.0f || y != 0.0f)
                {
                    angle = Mathf.Atan2(y, x);

                    displaceAngleVector.x = distance * Mathf.Sin(angle);
                    displaceAngleVector.y = distance * -Mathf.Cos(angle);

                    currentChild.transform.position = transform.position + displaceAngleVector;
                }
            }

            if (coolDownTimer >= coolDown)
            {
                if (Input.GetButton("Attack1"))
                {
                    currentChild.GetComponent<TrailRenderer>().enabled = true;
                    coolDownTimer = 0.0f;
                    currentChild.SetParent(null);
                    Vector3 heading = currentChild.transform.position - transform.position;
                    currentChild.GetComponent<Rigidbody>().velocity = heading * launchStrength;
                }
            }

            coolDownTimer += Time.deltaTime;
        }
        
    }

    public void AbleAutoAim()
    {
        autoAim = !autoAim;
    }
}
