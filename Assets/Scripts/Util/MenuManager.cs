using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public SpriteRenderer start;

    public SpriteRenderer quit;

    public Color selectedColor;
    public Color unselectedColor;

    private bool isStartSelected = true;

    public float duration = 5f;
    public float strength = 0.5f;

    public List<Transform> pathStart;
    public List<Transform> pathQuit;

    private Vector3[] startV;
    private Vector3[] quitV;
    
    private void Start()
    {
        startV = new Vector3[pathStart.Count];
        quitV = new Vector3[pathQuit.Count];

        for (int i = 0; i < pathStart.Count; i++)
        {
            startV[i] = pathStart[i].position;
        }

        for (int i = 0; i < pathQuit.Count; i++)
        {
            quitV[i] = pathQuit[i].position;
        }
        
        UpdateSprites();
        ShakeDaBooty();
    }

    private void ShakeDaBooty()
    {
        start.transform.DOPath(startV, duration);
        quit.transform.DOPath(quitV, duration).OnComplete(ShakeDaBooty);
    }

    private void UpdateSprites()
    {
        if (isStartSelected)
        {
            start.color = selectedColor;
            quit.color = unselectedColor;
        }
        else
        {
            start.color = unselectedColor;
            quit.color = selectedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftStick.down.wasPressedThisFrame)
        {
            isStartSelected = !isStartSelected;
            UpdateSprites();
        }
        else if (Gamepad.current.leftStick.up.wasPressedThisFrame)
        {
            isStartSelected = !isStartSelected;
            UpdateSprites();
        }

        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            if(isStartSelected)
                SceneManager.LoadScene(1);
            else
                Application.Quit();
        }
    }
}
