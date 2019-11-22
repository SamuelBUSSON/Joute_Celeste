using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;

        if (Health >= 0)
        {
            if (thresholds[indexThreshold].health >= Health)
            {
                
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
