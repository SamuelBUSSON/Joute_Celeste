using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.VFX;

public class Displacement : MonoBehaviour
{

    public float speed = 0.1f;
    public float maxVelocityChange = 10.0f;

    [Header("Dash")]   
    public C_Dash[] dashs;
    [Tooltip("The time you have to do the next dash")]
    public float dashCoolDown = 1.0f;

    [Header("Dash Stun")]
    public float stunTime = 2.0f;
    public float onStunSpeedDivide = 5.0f;

    [Header("Zone Slow")]
    public float slowStrength = 3.0f;

    public VisualEffect dashEffect;


    private float dashCoolDownTimer = 0.0f;
    private float stunTimer = 0.0f;
    private int currentDash = 0;
    private bool isStun = false;    

    private Rigidbody2D rigidbody2d;

    private PlayerInput input;

    private Vector3 movement;
    private bool isDashing;

    // Start is called before the first frame update
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        
        input.actions.Enable();
        input.currentActionMap["Movement"].performed += context => OnMovement(context);
        input.currentActionMap["Movement"].canceled += context => OnMovement(context);

        input.currentActionMap["Dash"].started += context => OnDash(context);
    }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();        
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        //Reads input
       movement = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        DashTimer();
    }

    private void DashTimer()
    {
        if(currentDash > 0)
        {
            dashCoolDownTimer += Time.deltaTime;

            if (dashCoolDownTimer > dashCoolDown)
            {
                currentDash = 0;
                dashCoolDownTimer = 0.0f;
            }
        }
    }

    private void Move()
    {
        if (!isDashing)
        {
            Vector2 targetVelocity = movement;
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *=  isStun ? speed/onStunSpeedDivide : speed ;

            Vector2 velocity = rigidbody2d.velocity;
            Vector2 velocityChange = (targetVelocity - velocity);

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);

            rigidbody2d.AddForce(velocityChange, ForceMode2D.Impulse);
        }

        if (isStun)
        {
            stunTimer += Time.deltaTime;
            if(stunTimer >= stunTime)
            {
                stunTimer = 0.0f;
                isStun = false;
            }
        }
        
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        if (!isStun)
        {
            Dash();
        }
    }

    private void DashCanceled()
    {
           GetComponentInChildren<PlayerZone>().ChangeSpeedObjectInZone(false);
           isDashing = false;        
    }

    private void Dash()
    {
        if (!isDashing)
        {
            if(movement.x != 0 || movement.y != 0)
            {
                isDashing = true;

                dashEffect.SendEvent("OnDash");

                GetComponentInChildren<PlayerZone>().ChangeSpeedObjectInZone(true);                
                rigidbody2d.DOMove(transform.position + movement * dashs[currentDash].dashStrength, dashs[currentDash].timeToReachDashPosition).OnComplete(() => DashCanceled()).SetEase(dashs[currentDash].easeDash);

                dashCoolDownTimer = 0.0f;
                ++currentDash;
                if(currentDash == dashs.Length)
                {
                    isStun = true;
                    currentDash = 0;
                }
            }
        }
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public bool IsStun()
    {
        return isStun;
    }


    [Serializable]
    public class C_Dash
    {
        public AnimationCurve easeDash;
        public float dashStrength;
        public float timeToReachDashPosition;
    }
}