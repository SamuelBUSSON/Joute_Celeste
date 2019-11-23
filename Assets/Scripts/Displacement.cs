using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Displacement : MonoBehaviour
{

    public float speed = 0.1f;
    public float maxVelocityChange = 10.0f;

    [Header("Dash")]
    public float dashStrength = 20.0f;
    public float timeToReachDashPosition = 0.3f;
    public AnimationCurve ease;

    private Rigidbody2D rigidbody2d;

    private PlayerInput input;

    private Vector3 movement;
    private bool dashTest;

    // Start is called before the first frame update
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        
        input.actions.Enable();
        input.currentActionMap["Movement"].performed += context => OnMovement(context);
        input.currentActionMap["Movement"].canceled += context => OnMovement(context);

        input.currentActionMap["Dash"].started += context => OnDash(context);
        input.currentActionMap["Dash"].canceled += context => OnDashCanceled();
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
    }

    private void Move()
    {
        if (!dashTest)
        {
            Vector2 targetVelocity = movement;
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            Vector2 velocity = rigidbody2d.velocity;
            Vector2 velocityChange = (targetVelocity - velocity);


            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);


            rigidbody2d.AddForce(velocityChange, ForceMode2D.Impulse);
        }

    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        Dash();
    }

    private void OnDashCanceled()
    {
       // dashTest = false;
    }

    private void Dash()
    {
        if (!dashTest)
        {
            if(movement.x != 0 || movement.y != 0)
            {
                dashTest = true;
                rigidbody2d.DOMove(transform.position + movement * dashStrength, timeToReachDashPosition).OnComplete(() => dashTest = false).SetEase(ease);

            }
        }
    }
}