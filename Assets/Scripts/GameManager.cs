﻿using System;
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
    public enum EState
    {
        Waiting,
        Starting,
        Ending
    }
    
    public static GameManager Instance;

    public GameObject player2Prefab;

    public GameObject explosionPrefab;

    private Vector3 player1StartPos;
    private Vector3 player2StartPos;

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
    [Tooltip("Le temps de passer à 1 d'opacité")]
    public float respawnDuration;

    public AnimationCurve easeOutCurve;

    public int roundTotal;
    private int roundCurrent;

    private int roundPlayer1;
    private int roundPlayer2;

    private RoundManager round;

    [NonSerialized]
    public EState state;

    private static readonly int Restart = Animator.StringToHash("Restart");

    public List<Sprite> roundImage;
    public Image roundImageCanvas;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoin;
            round = GetComponent<RoundManager>();

            roundCurrent = 1;

            roundPlayer1 = roundPlayer2 = 0;

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].gameObject.SetActive(false);
            }

            state = EState.Waiting;
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

            AkSoundEngine.PostEvent("Play_Musique", gameObject);

            StartPhase();
        }
        playerIndex++;
    }

    private void PlaySound()
    {
        CameraManager.Instance.Shake(5.0f, 5.0f, 1f);
        AkSoundEngine.PostEvent("Play_Explosion_Comete", gameObject);
        Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity);
    }

    private void StartPhase()
    {
        Debug.Log("start phase");        

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(player1.transform.DOMove(new Vector3(-0.2f, 0.0f), 0.5f));
        mySequence.Join(player2.transform.DOMove(new Vector3(0.2f, 0.0f), 0.5f));
        mySequence.Append(DOVirtual.DelayedCall(0, () => PlaySound()));
        mySequence.Append(player1.transform.DOMove(new Vector3(-5.0f, 0.0f), 1f));
        mySequence.Join(player2.transform.DOMove(new Vector3(5.0f, 0.0f), 1f));

        round.timer = 0;
        round.enabled = true;

        switch (roundCurrent)
        {
            case 1:
                AkSoundEngine.PostEvent("Play_Round1", gameObject);
                break;

            case 2:
                AkSoundEngine.PostEvent("Play_Round2", gameObject);
                break;

            case 3:
                AkSoundEngine.PostEvent("Play_Round3", gameObject);
                break;
        }

        roundImageCanvas.sprite = roundImage[roundCurrent - 1];
        roundImageCanvas.gameObject.SetActive(true);

        Color c = roundImageCanvas.color;

        DOVirtual.Float(0, 1, 1f, value =>
        {
            c.a = value;
            roundImageCanvas.color = c;
        }).OnComplete(() => DOVirtual.Float(1, 0, 0.5f, value =>
        {
            c.a = value;
            roundImageCanvas.color = c;
        })).OnComplete(() => roundImageCanvas.gameObject.SetActive(false));

        if (roundCurrent != 1)
        {
            ResetPlayer(player1);
            ResetPlayer(player2);
        }
        
        
        StartCoroutine(StartPhaseExplosion());
    }

    private void ResetPlayer(PlayerController player)
    {
        var phealth = player.GetComponent<PlayerHealth>();        

        // if (phealth.isDead)
        //     phealth.GetComponent<Animator>().SetTrigger(Restart);

        StartCoroutine(StartReset(phealth));

        phealth.Reset();
    }

    private IEnumerator StartReset(PlayerHealth phealth)
    {
        yield return new WaitForFixedUpdate();
        phealth.GetComponent<Animator>().SetTrigger(Restart);
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

        state = EState.Starting;
    }


    private IEnumerator SetupPlayers()
    {
        yield return new WaitForFixedUpdate();
        player1.GetComponent<ObjectHandler>().SetEnemyPos(player2.transform);
        player2.GetComponent<ObjectHandler>().SetEnemyPos(player1.transform);
    }

    public void TimeUp()
    {
        //spawn death sphere
        Debug.Log("Death sphere spawning");

        round.enabled = false;
    }
    

    public void Draw()
    {
        Debug.Log("Draw");
        state = EState.Ending;
        NextRound();
        
        RespawnPlayer(player1, player1StartPos);
        RespawnPlayer(player2, player2StartPos);
        
        StartPhase();
    }

    public void WinLoose(int looserIndex)
    {
        if (looserIndex == player1.playerIndex)
        {
            if (player2.GetComponent<PlayerHealth>().isDead)
            {
                Draw();
                return;
            }
        }
        else
        {
            if (player1.GetComponent<PlayerHealth>().isDead)
            {
                Draw();
                return;
            }
        }

        Debug.Log("Player " + looserIndex + " looses !");
        state = EState.Ending;

        NextRound();

        if (player1.playerIndex == looserIndex)
        {
            RespawnPlayer(player1, player1StartPos);
            roundPlayer2++;
        }
        else
        {
            RespawnPlayer(player2, player2StartPos);
            roundPlayer1++;
        }
            

        StartCoroutine(StartPhaseAfterTime(respawnDuration));
    }

    private IEnumerator StartPhaseAfterTime(float f)
    {
        yield return new WaitForSeconds(f);
        StartPhase();
    }

    private void NextRound()
    {
        roundCurrent++;
        
        foreach (ProjectileSpawner spawner in spawners)
        {
            for (int i = 0; i < spawner.transform.childCount; i++)
            {
                if (!spawner.transform.GetChild(i).GetComponent<StarSpawner>())
                {
                    Destroy(spawner.transform.GetChild(i).gameObject);
                }
                else
                {
                    spawner.transform.GetChild(i).GetComponent<StarSpawner>().ResetTimer();
                }

            }
        }

        Debug.Log("Round " + roundCurrent);
        
        if (roundCurrent == roundTotal)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (roundPlayer1 > roundPlayer2)
        {
            Destroy(player2.gameObject);
            Debug.Log("player 1 win !");
        }
        else if (roundPlayer2 > roundPlayer1)
        {
            Destroy(player1.gameObject);
            Debug.Log("player 2 win");
        }
        else
        {
            Debug.Log("Draw");
        }
        
        
        Debug.Log("End of the game !");
        
        RespawnPlayer(player1, player1StartPos);
        RespawnPlayer(player2, player2StartPos);
        
        StartPhase();
    }

    private void RespawnPlayer(PlayerController player, Vector3 position)
    {
        //reduce opacity to zero, set position, opacity back to 1

        var spr = player.GetComponent<SpriteRenderer>();
        var c = spr.color;
        c.a = 0;
        
        spr.color = c;

        spr.transform.position = position;

        DOVirtual.Float(0, 1, respawnDuration, value =>
        {
            c.a = value;
            spr.color = c;
        });
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
