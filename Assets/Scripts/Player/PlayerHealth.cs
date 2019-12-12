using System;
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
            TakeDamage(1.0f);
        }
    }

    public void GiveHealth(float amount)
    {
        Health += amount;

        //AkSoundEngine.SetSwitch("Witch_Aura", playerController?.playerIndex == 1 ? "Aura1" : "Aura2", gameObject);


        AkSoundEngine.PostEvent("Play_Player_Heal", gameObject);

        if (Health > maxHealth)
            Health = maxHealth;

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

            AkSoundEngine.SetSwitch("Witch_Aura", playerController?.playerIndex == 1  ? "Aura1" : "Aura2", gameObject);

            if(!playerMovement.isAi)
                AttractZone.GetComponent<Animator>().SetFloat("Health", Health);
            else
                GetComponent<Animator>().SetFloat("Health", Health);

            if (Health >= 0 && indexThreshold < thresholds.Count)
            {
                if (thresholds[indexThreshold].health >= Health)
                {
                    if (++indexThreshold < thresholds.Count)
                    {
                        playerMovement.speed *= thresholds[indexThreshold].speedMultiplier;
                        _objectHandler.damageMultiplier = thresholds[indexThreshold].damageMultiplier;
                        
                        if (indexThreshold == 1)
                        {
                            AkSoundEngine.SetSwitch("Aura_State", "State1to2", gameObject);
                            if(!playerMovement.isAi)
                                AttractZone.transform.DOScale(AttractZone.transform.localScale * attractRange, 0.5f);
                        }
                        else if (indexThreshold == 2)
                        {
                            AkSoundEngine.SetSwitch("Aura_State", "State2to3", gameObject);
                            playerMovement.dashCoolDown /= 2;
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
