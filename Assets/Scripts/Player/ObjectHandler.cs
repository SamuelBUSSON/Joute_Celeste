﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Player;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class ObjectHandler : MonoBehaviour
{

    public float distance = 1.0f;

    public float launchStrength = 2.0f;
    [NonSerialized] public float damageMultiplier = 1f;

    public float knockbackForce = 2.0f;

    public Transform enemyPos;

    private Transform handledObject;
    private float angle = 0.0f;
    private Vector3 displaceAngleVector;

    private float coolDownTimer = 0.0f;

    private PlayerInput input;

    private bool autoAim = false;

    private Displacement playerMovement;
    private PlayerZone playerZone;
    private PlayerHealth playerHealth;

    private PlayerController controller;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();


        input.actions.Enable();

        input.currentActionMap["Fire"].canceled += context => OnFire(context);

        input.currentActionMap["Aim"].performed += context => OnAim(context);
        input.currentActionMap["Aim"].canceled += context => OnAutoAim(context);
        
        input.currentActionMap["HoldLv1"].performed += OnHoldLv1;
        input.currentActionMap["HoldLv2"].performed += OnHoldLv2;

        input.currentActionMap["CaugthLv1"].performed += OnDrainStar;

        enemyPos = transform;
        controller = GetComponent<PlayerController>();

        playerHealth = GetComponent<PlayerHealth>();
    }
    

    private void OnHoldLv2(InputAction.CallbackContext obj)
    {
        if (handledObject)
        {
            Projectile proj = handledObject.GetComponent<Projectile>();

            AkSoundEngine.PostEvent("Play_Player_Charge", gameObject);

            VisualEffect fx = proj.GetComponentInChildren<VisualEffect>();

            fx.SetFloat("Radius", proj.size + 0.5f);
            fx.SendEvent("OnCast");


            proj?.GetComponent<SpriteRenderer>().material.DOFloat((proj.GetComponent<SpriteRenderer>().material.GetFloat("_PowerColor") * 1.5f), "_PowerColor", 0.3f);

            if (proj.type != EProjectileType.PLANET)
            {
                proj.SetThresholdLevel(1);
            }
        }
    }

    private void OnDrainStar(InputAction.CallbackContext obj)
    {
        Projectile proj = handledObject.GetComponent<Projectile>();
        if (proj.type == EProjectileType.STAR)
        {
            DrainStar(proj);
        }
    }


    private void DrainStar(Projectile proj)
    {
        handledObject = null;
        playerHealth.GiveHealth(playerHealth.maxHealth / 3);
        Destroy(proj.gameObject);
    }

    private void OnHoldLv1(InputAction.CallbackContext obj)
    {
        if (handledObject)
        {
            AkSoundEngine.PostEvent("Play_Player_Charge", gameObject);

            Projectile proj = handledObject.GetComponent<Projectile>();

            proj.GetComponentInChildren<VisualEffect>().SendEvent("OnCast");

            proj?.GetComponent<SpriteRenderer>().material.DOFloat( (proj.GetComponent<SpriteRenderer>().material.GetFloat("_PowerColor") * 1.5f), "_PowerColor" ,0.3f);

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

        playerMovement = GetComponent<Displacement>();

        playerZone = GetComponentInChildren<PlayerZone>();
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
               handledObject.transform.position = transform.position + displaceAngleVector * handledObject.GetComponent<Projectile>().size; ;
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
        if (!playerMovement.IsDashing())
        {
            Fire();
        }
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
            FireObject();
        }
        else
        {
            handledObject = playerZone.GetNearestObjectInZone();

            if (handledObject)
            {
                SetObjectHandled(handledObject);
                handledObject.gameObject.layer = 10;
                FireObject();
            }
        }
    }

    private void FireObject()
    {
        CameraManager.Instance.Shake(5.0f, 5.0f, 0.1f);
        //TODO: check findgamepad id

        if (input.user.pairedDevices[0] is Gamepad pad)
        {
            CameraManager.Instance.Vibrate(0.8f, 0.0f, 0.1f, pad);
        }

        

        handledObject.SetParent(null);

        Projectile projectile = handledObject.GetComponent<Projectile>();
        projectile.isLaunched = true;
        projectile.tag = "Untagged";
        projectile.currentDamage *= damageMultiplier;
        projectile.playerIndex = controller.playerIndex;


        AkSoundEngine.SetSwitch("Choix_Astres", projectile.type == EProjectileType.PLANET ? "Planete" : projectile.type == EProjectileType.STAR ? "Etoile" : "Comete", gameObject);
        AkSoundEngine.PostEvent("Play_Player_Fire", gameObject);


        Vector3 heading = (handledObject.transform.position - transform.position).normalized;

        handledObject.GetComponent<Rigidbody2D>().velocity = projectile.speed * launchStrength * heading;

        VisualEffect fx = handledObject.GetComponent<VisualEffect>();
        fx.SetBool("SpawnRate", false);

        handledObject.GetComponentsInChildren<VisualEffect>()[1].enabled = true;

        Vector2 v1 = transform.position;
        Vector2 v2 = heading;

        transform.DOMove(v1 - v2 * knockbackForce, 0.05f);

        handledObject = null;
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


            objectToThrow?.GetComponent<SpriteRenderer>().material.SetInt("_HighLigth", 0);

            handledObject = objectToThrow;

            handledObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            handledObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            handledObject.GetComponent<Rigidbody2D>().inertia = 0;

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
