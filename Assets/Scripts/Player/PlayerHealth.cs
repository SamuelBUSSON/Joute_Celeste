using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Serializable]
    public class SHealthThreshold //TODO: maybe add a range to enlarge the entity
    {
        public float health;
        public float speed;
    }

    public List<SHealthThreshold> thresholds;

    public float attractRange;
    public GameObject AttractZone;

    public float Health;

    private int indexThreshold;

    private Slider healthSlider;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private int playerIndex;

    private Displacement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        if(playerController?.playerIndex == 1)
        {
            GameObject p0 = GameManager.Instance.player2.gameObject;
            p0.GetComponent<ObjectHandler>().SetEnemyPos(transform);
            GetComponent<ObjectHandler>().SetEnemyPos(p0.transform);
        }
        
        healthSlider = playerController?.playerIndex == 0
            ? GameManager.Instance.PlayerSliderHealth1
            : GameManager.Instance.PlayerSliderHealth2;

        healthSlider.maxValue = Health;
        healthSlider.value = Health;

        playerMovement = GetComponent<Displacement>();
    }

    public void TakeDamage(float amount)
    {
        if (!playerMovement.IsDashing())
        {
            Health -= amount;
            healthSlider.value = Health;

            if (Health >= 0)
            {
                if (thresholds[indexThreshold].health >= Health)
                {
                    if (++indexThreshold < thresholds.Count)
                    {
                        playerMovement.speed *= thresholds[indexThreshold].speed;

                        if (indexThreshold == 1)
                        {
                            AttractZone.transform.localScale *= attractRange;
                        }
                        else if (indexThreshold == 2)
                        {
                            playerMovement.dashCoolDown /= 2;
                        }
                    }
                }
            }
            else
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //TODO
    }
}
