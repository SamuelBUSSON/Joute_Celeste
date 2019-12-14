using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player2Prefab;

    [NonSerialized]
    public bool canMove = false;
    
    [NonSerialized]
    public PlayerController player1;
    
    [NonSerialized]
    public PlayerController player2;
    
    public Slider PlayerSliderHealth1;
    public Slider PlayerSliderHealth2;

    public CinemachineTargetGroup targetGroup;

    public ProjectileSpawner[] spawners;

    [NonSerialized]
    public int playerIndex = -1;

    [Header("Round")] 
    public float moveToCenterDuration;

    public Animator explosionAnim;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoin;

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].gameObject.SetActive(false);
            }
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
        { 
            player1 = obj.GetComponent<PlayerController>();
            GetComponent<PlayerInputManager>().playerPrefab = player2Prefab;
        }
        else
        {
            player2 = obj.GetComponent<PlayerController>();
            StartCoroutine(SetupPlayers());

            StartPhase();
        }
        playerIndex++;
    }

    private void StartPhase()
    {
        StartCoroutine(StartPhaseExplosion());
        player1.transform.DOMove(new Vector3(0.05f, 0.05f), moveToCenterDuration);
        player2.transform.DOMove(new Vector3(0.05f, 0.05f), moveToCenterDuration);
    }

    private IEnumerator StartPhaseExplosion()
    {
        yield return new WaitForSeconds(moveToCenterDuration);
        //play explosion
        
        explosionAnim.Play("Boom");

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].gameObject.SetActive(true);
        }

        canMove = true;
    }


    private IEnumerator SetupPlayers()
    {
        yield return new WaitForFixedUpdate();
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
