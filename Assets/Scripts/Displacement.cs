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
    public float soapStrength = 10f;

    [Header("Dash")]   
    public C_Dash[] dashs;
    [Tooltip("The time you have to do the next dash")]
    public float dashCoolDown = 1.0f;
    public VisualEffect dash_FX;
    public float impusleStrength = 10.0f;

    [Header("Dash Stun")]
    public float stunTime = 2.0f;
    public float onStunSpeedDivide = 5.0f;

    [Header("Zone Slow")]
    public float slowStrength = 3.0f;    


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
        input.currentActionMap["Movement"].started += context => OnStartMovement(context);
        input.currentActionMap["Movement"].performed += context => OnMovement(context);
        input.currentActionMap["Movement"].canceled += context => OnMovementCancel(context);

        input.currentActionMap["Dash"].started += context => OnDash(context);
    }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();   
    }

    private void OnStartMovement(InputAction.CallbackContext obj)
    {
        if(GameManager.Instance.canMove)
            AkSoundEngine.PostEvent("Play_Player_Move_Solo", gameObject);
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        if(GameManager.Instance.canMove)
            movement = obj.ReadValue<Vector2>();      
    }

    private void OnMovementCancel(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.canMove)
        {
            movement = obj.ReadValue<Vector2>();     
            AkSoundEngine.PostEvent("Stop_Player_Move_Solo", gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(false);

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

    private void Move(bool isCancel)
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
        if (GameManager.Instance.canMove)
        {
            if (!isStun)
            {
                Dash();
            }
        }
        
    }

    private void DashCanceled()
    {
        GetComponentInChildren<PointEffector2D>().forceMagnitude = -10;
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

                AkSoundEngine.PostEvent("Play_Player_Dash", gameObject);

                PlayerZone pl = GetComponentInChildren<PlayerZone>();
                pl.ChangeSpeedObjectInZone(true);

                if(currentDash >= 1)
                {
                    AkSoundEngine.PostEvent("Play_Player_Dash2", gameObject);
                }

                GetComponentInChildren<PointEffector2D>().forceMagnitude = 100;

                rigidbody2d.DOMove(transform.position + movement * dashs[currentDash].dashStrength, dashs[currentDash].timeToReachDashPosition).OnComplete(() => DashCanceled()).SetEase(dashs[currentDash].easeDash).OnStart(() => DashEffect());

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

    private void DashEffect()
    {
        dash_FX.SendEvent("OnDash");
        dash_FX.SetFloat("RotateAngle", Mathf.Atan2(-movement.normalized.x, -movement.normalized.y));
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