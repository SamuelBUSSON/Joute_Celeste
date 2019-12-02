using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityTemplateProjects.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [NonSerialized]
    public PlayerController player1;
    
    [NonSerialized]
    public PlayerController player2;
    
    public Slider PlayerSliderHealth1;
    public Slider PlayerSliderHealth2;

    public CinemachineTargetGroup targetGroup;

    private PlayerInputManager inputManager;
    [NonSerialized]
    public int playerIndex = -1;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoin;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnPlayerJoin(PlayerInput obj)
    {
        targetGroup.AddMember(obj.transform, 1, 1);
        
        if (playerIndex == -1)
            player1 = obj.GetComponent<PlayerController>();
        else
        {
            player2 = obj.GetComponent<PlayerController>();
        }
        playerIndex++;
    }
}
