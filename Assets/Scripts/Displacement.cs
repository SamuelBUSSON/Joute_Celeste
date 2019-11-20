using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Displacement : MonoBehaviour
{

    public float speed = 0.1f;

    private PlayerInput input;

    private Vector3 movement;

    // Start is called before the first frame update
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        
        input.actions.Enable();
        input.currentActionMap["Movement"].performed += context => OnMovement(context);
        input.currentActionMap["Movement"].canceled += context => OnMovement(context);
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        //Reads input
       movement = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += movement * speed;
        
        /*if (Input.GetAxis("Horizontal") != 0)
        {
            h = Input.GetAxis("Horizontal");
        }
        else
        {
            h = 0;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            v = Input.GetAxis("Vertical");
        }
        else
        {
            v = 0;
        }

        Vector3 displaceVector = new Vector3(h, v, 0).normalized;

        transform.position += displaceVector * speed;*/

    }
}
