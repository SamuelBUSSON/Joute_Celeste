using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.VFX;

public class ObjectHandler : MonoBehaviour
{
    public GameObject starPrefab;

    public float distance = 1.0f;
    public float coolDown = 0.2f;

    public float launchStrength = 2.0f;    

    public Transform enemyPos;

    private Transform handledObject;
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

        if (handledObject)
        {
            Aim();

            if (Input.GetButton("Attack1"))
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        if (coolDownTimer >= coolDown)
        {

            coolDownTimer = 0.0f;
            handledObject.SetParent(null);
            Vector3 heading = handledObject.transform.position - transform.position;
            handledObject.GetComponent<Rigidbody2D>().velocity = heading * launchStrength;

            Destroy(handledObject.gameObject, 3.0f);

            handledObject.GetComponentInChildren<VisualEffect>().enabled = true;

            handledObject = null;
        }
        coolDownTimer += Time.deltaTime;

    }

    private void Aim()
    {
        float x = Input.GetAxis("ControllerVertical");
        float y = Input.GetAxis("ControllerHorizontal");

        if (x <= 0.1f && y <= 0.1f)
        {
            Vector3 heading = enemyPos.position - transform.position;

            angle = Mathf.Atan2(heading.normalized.x, heading.normalized.y);

            displaceAngleVector.x = distance * Mathf.Sin(angle);
            displaceAngleVector.y = distance * Mathf.Cos(angle);

            handledObject.transform.position = transform.position + displaceAngleVector;
        }
        else
        {
            if (x != 0.0f || y != 0.0f)
            {
                angle = Mathf.Atan2(y, x);

                displaceAngleVector.x = distance * Mathf.Sin(angle);
                displaceAngleVector.y = distance * -Mathf.Cos(angle);

                handledObject.transform.position = transform.position + displaceAngleVector;
            }
        }
    }

    public void SetObjectHandled(Transform objectToThrow)
    {
        handledObject = objectToThrow;
        handledObject.SetParent(transform);
    }

    public Transform GetObjectHandled()
    {
        return handledObject;
    }

}
