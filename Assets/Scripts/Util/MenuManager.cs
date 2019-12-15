using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Util;

public class MenuManager : MonoBehaviour
{
    private int index;
    public List<ButtonSelector> buttons;
    
    private Vector3[] startV;
    private Vector3[] quitV;
    
    private void Start()
    {
        index = 0;
        buttons[0].ToogleFocus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftStick.down.wasPressedThisFrame)
        {
            buttons[index].ToogleFocus();
            index = (index + 1) % buttons.Count;
            buttons[index].ToogleFocus();
        }
        else if (Gamepad.current.leftStick.up.wasPressedThisFrame)
        {
            print("yes");
            buttons[index].ToogleFocus();
            index--;
            if (index == -1)
                index = buttons.Count - 1;
            buttons[index].ToogleFocus();
        }
        
        if(Gamepad.current.aButton.wasPressedThisFrame)
            buttons[index].Interact();        
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
