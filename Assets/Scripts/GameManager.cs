using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [NonSerialized]
    public int playerIndex = -1;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoin;
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
            SetupPlayers();
        }
        playerIndex++;
    }
    

    private void SetupPlayers()
    {
        player1.GetComponent<ObjectHandler>().SetEnemyPos(player2.transform);
        player2.GetComponent<ObjectHandler>().SetEnemyPos(player1.transform);
    }

    public void TimeUp()
    {
        PlayerHealth p1 = player1.GetComponent<PlayerHealth>();
        PlayerHealth p2 = player2.GetComponent<PlayerHealth>();

        if (p1.Health > p2.Health)
        { 
            p1.Die();
        }
        else if (p1.Health < p2.Health)
        { 
            p2.Die();
        }
        else
        {
            Draw();
        }
    }

    public void Draw()
    {
        Debug.Log("Draw");
    }

    public void WinLoose(int looserIndex)
    {
        Debug.Log("Player " + looserIndex + " looses !");
    }
}
