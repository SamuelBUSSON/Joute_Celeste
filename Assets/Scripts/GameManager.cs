using System;
using System.Collections;
using System.Collections.Generic;
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
        print("yes");
        if (playerIndex == -1)
            player1 = obj.GetComponent<PlayerController>();
        else
        {
            player2 = obj.GetComponent<PlayerController>();
        }
        playerIndex++;
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
