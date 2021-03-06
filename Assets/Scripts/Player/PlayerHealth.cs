﻿using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Serializable]
    public class SHealthThreshold //TODO: maybe add a range to enlarge the entity
    {
        public float health;
        public float speedMultiplier;
        public float damageMultiplier;
    }

    [Header("Thresholds")]
    public List<SHealthThreshold> thresholds;

    public float attractRange;
    public GameObject AttractZone;

    [Header("Death")]
    public float deathRange;
    public float deathDamage;

    [Header("Health")]
    public float Health;

    public AnimatorOverrideController aura3Controller;
    
    [NonSerialized]
    public float maxHealth;

    private int indexThreshold = 0;

    private Slider healthSlider;

    private PlayerController playerController;
    private ObjectHandler _objectHandler;
    

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    

    private Displacement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Health;
        
        if(playerController?.playerIndex == 1)
        {
            GameObject p0 = GameManager.Instance.player2.gameObject;
            p0.GetComponent<ObjectHandler>().SetEnemyPos(transform);
            GetComponent<ObjectHandler>().SetEnemyPos(p0.transform);
        }
        
        healthSlider = playerController?.playerIndex == 0
            ? GameManager.Instance.PlayerSliderHealth1
            : GameManager.Instance.PlayerSliderHealth2;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = Health;

        playerMovement = GetComponent<Displacement>();
        _objectHandler = GetComponent<ObjectHandler>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDamage(10.0f);
        }
    }

    public void GiveHealth(float amount)
    {
        Health += amount;

        AkSoundEngine.PostEvent("Play_Player_Heal", gameObject);

        if (Health > maxHealth)
            Health = maxHealth;

        AttractZone.GetComponent<Animator>().SetFloat("Health", Health);

        if(thresholds[indexThreshold].health <= Health)
        {
            if (--indexThreshold >= 0)
            {
                playerMovement.speed /= thresholds[indexThreshold].speedMultiplier;
                _objectHandler.damageMultiplier = thresholds[indexThreshold].damageMultiplier;

                if (indexThreshold == 0)
                {
                    AkSoundEngine.SetSwitch("Aura_State", "State2to1", gameObject);
                    AttractZone.transform.DOScale(AttractZone.transform.localScale / attractRange, 0.5f);
                }
                else if (indexThreshold == 1)
                {
                    AkSoundEngine.SetSwitch("Aura_State", "State3to2", gameObject);
                    playerMovement.dashCoolDown *= 2;
                }
            }
        }


        healthSlider.value = Health;
    }

    /// <summary>
    /// Handles damage, threshold changes and death
    /// </summary>
    /// <param name="amount">Amount of damage received</param>
    /// <returns></returns>
    public bool TakeDamage(float amount)
    {
        if (!playerMovement.IsDashing())
        {
            Health -= amount;
            healthSlider.value = Health;

            AkSoundEngine.PostEvent("Play_Player_Hit", gameObject);

            float shakeValue = Mathf.Lerp(2.0f, 20.0f, Mathf.InverseLerp(5, 50, amount));

            CameraManager.Instance.Shake(shakeValue, shakeValue, 0.1f);            

            AkSoundEngine.SetSwitch("Witch_Aura", playerController?.playerIndex == 1  ? "Aura1" : "Aura2", gameObject);

            AttractZone.GetComponent<Animator>().SetFloat("Health", Health);

            if (Health >= 0 && indexThreshold < thresholds.Count)
            {
                if (thresholds[indexThreshold].health >= Health)
                {
                    if (indexThreshold + 1 < thresholds.Count)
                    {
                        indexThreshold++;
                        playerMovement.speed *= thresholds[indexThreshold].speedMultiplier;
                        _objectHandler.damageMultiplier = thresholds[indexThreshold].damageMultiplier;
                        
                        if (indexThreshold == 1)
                        {
                            AkSoundEngine.SetSwitch("Aura_State", "State1to2", gameObject);
                           AttractZone.transform.DOScale(AttractZone.transform.localScale * attractRange, 0.5f);
                        }
                        else if (indexThreshold == 2)
                        {
                            AkSoundEngine.SetSwitch("Aura_State", "State2to3", gameObject);
                            playerMovement.dashCoolDown /= 2;

                            GetComponent<Animator>().runtimeAnimatorController = aura3Controller;
                        }
                    }
                    AkSoundEngine.PostEvent("Play_Aura1_or_2", gameObject);
                }
            }
            else
            {
                Die();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Circle ray cast to inflict damage to the other enemy, checks if draw
    /// </summary>
    public void Die()
    {
        Vector2 position = transform.position;
        var hits = Physics2D.CircleCastAll(position, deathRange, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController playerCtrl = hit.transform.GetComponent<PlayerController>();

            if (playerCtrl && playerCtrl.playerIndex != playerController.playerIndex)
            {
                if (playerCtrl.GetComponent<PlayerHealth>().Health - deathDamage <= 0)
                {
                    GameManager.Instance.Draw();
                }
                else
                {
                    GameManager.Instance.WinLoose(playerController.playerIndex);
                }
            }
        }
    }
}
