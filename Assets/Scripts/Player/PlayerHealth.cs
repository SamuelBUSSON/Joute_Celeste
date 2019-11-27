using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityTemplateProjects.Player;

public class PlayerHealth : MonoBehaviour
{
    [Serializable]
    public struct SHealthThreshold //TODO: maybe add a range to enlarge the entity
    {
        public int health;
    }

    public List<SHealthThreshold> thresholds;
    
    public int Health;

    private int indexThreshold;

    private Slider healthSlider;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerController.playerIndex == 1)
        {
            GameObject p0 = GameManager.Instance.player2.gameObject;
            p0.GetComponent<ObjectHandler>().SetEnemyPos(transform);
            GetComponent<ObjectHandler>().SetEnemyPos(p0.transform);
        }
        
        healthSlider = playerController.playerIndex == 0
            ? GameManager.Instance.PlayerSliderHealth1
            : GameManager.Instance.PlayerSliderHealth2;

        healthSlider.maxValue = Health;
        healthSlider.value = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        healthSlider.value = Health;

        if (Health >= 0)
        {
            if (thresholds[indexThreshold].health >= Health)
            {
                if (++indexThreshold < thresholds.Count)
                {
                    //TODO: change range or whatnot
                }
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        //TODO
    }
}
