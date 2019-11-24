using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GameManager.Instance.playerIndex;

        gameObject.tag = "Player" + playerIndex;
        if(playerIndex == 1)
        {
            GameObject p0 = GameObject.FindGameObjectWithTag("Player0");
            p0.GetComponent<ObjectHandler>().SetEnemyPos(transform);
            GetComponent<ObjectHandler>().SetEnemyPos(p0.transform);
        }
        
        healthSlider = playerIndex == 0
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

    private int GetPlayerIndex()
    {
        return playerIndex;
    }
}
