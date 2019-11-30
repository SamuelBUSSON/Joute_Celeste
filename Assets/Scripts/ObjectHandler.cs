﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.VFX;
using UnityEngine.InputSystem;

public class ObjectHandler : MonoBehaviour
{

    public float distance = 1.0f;
    public float coolDown = 0.2f;

    public float launchStrength = 2.0f;    

    public Transform enemyPos;

    private Transform handledObject;
    private float angle = 0.0f;
    private Vector3 displaceAngleVector;

    private float coolDownTimer = 0.0f;

    private PlayerInput input;

    private bool autoAim = false;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        input.actions.Enable();

        input.currentActionMap["Fire"].canceled += context => OnFire(context);

        input.currentActionMap["Aim"].performed += context => OnAim(context);
        input.currentActionMap["Aim"].canceled += context => OnAutoAim(context);
        
        input.currentActionMap["HoldLv1"].performed += OnHoldLv1;
        input.currentActionMap["HoldLv2"].performed += OnHoldLv2;

        enemyPos = transform;


    }

    private void OnHoldLv2(InputAction.CallbackContext obj)
    {
        print("HoldLv2");
        
        if (handledObject)
        {
            Projectile proj = handledObject.GetComponent<Projectile>();

            if (proj.type != EProjectileType.PLANET)
            {
                proj.SetThresholdLevel(1);
            }
        }
    }

    private void OnHoldLv1(InputAction.CallbackContext obj)
    {
        print("HoldLv1");

        if (handledObject)
        {
            Projectile proj = handledObject.GetComponent<Projectile>();

            if (proj.type != EProjectileType.PLANET)
            {
                proj.SetThresholdLevel(0);
            }
        }
    }

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
            if (autoAim)
            {
                Aim(Vector2.zero, true);
            }
            handledObject.transform.position = transform.position + displaceAngleVector;

        }


        coolDownTimer += Time.deltaTime;       
    }

    private void LateUpdate()
    {
        if (handledObject)
        {
            handledObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        Fire();
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        autoAim = false;
        if (handledObject)
        {
            Aim(obj.ReadValue<Vector2>(), false);
        }
    }

    private void OnAutoAim(InputAction.CallbackContext obj)
    {
        autoAim = true;
    }



    private void Fire()
    {
        if (handledObject)
        {
            if (coolDownTimer >= coolDown)
            {
                coolDownTimer = 0.0f;
                handledObject.SetParent(null);

                Projectile projectile = handledObject.GetComponent<Projectile>();
                projectile.isLaunched = true;

                Vector3 heading = handledObject.transform.position - transform.position;
                handledObject.GetComponent<Rigidbody2D>().velocity = projectile.speed * launchStrength * heading;

               // Destroy(handledObject.gameObject, 3.0f);

                handledObject.GetComponentInChildren<VisualEffect>().enabled = true;
                

                handledObject = null;
            }
        }
    }

    private void Aim(Vector2 aimDirection, bool autoAim)
    {
            Vector3 heading = enemyPos ? enemyPos.position - transform.position : Vector3.zero;

            float x = aimDirection.x;
            float y = aimDirection.y;

            angle = autoAim ? Mathf.Atan2(heading.normalized.x, heading.normalized.y) : Mathf.Atan2(x, y);

            displaceAngleVector.x = distance * Mathf.Sin(angle);
            displaceAngleVector.y = distance * Mathf.Cos(angle);            

    }

    public void SetObjectHandled(Transform objectToThrow)
    {
            handledObject = objectToThrow;

            handledObject.gameObject.layer = 10;     

            handledObject.SetParent(transform);

            Aim(Vector2.zero, true );
    }

    public Transform GetObjectHandled()
    {
        return handledObject;
    }

    public void SetEnemyPos(Transform t)
    {
        enemyPos = t;
    }


}
