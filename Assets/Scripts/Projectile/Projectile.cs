﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTemplateProjects.Player;

public enum EProjectileType
{
    ASTEROID = 0,
    PLANET,
    STAR
}

public class Projectile : MonoBehaviour
{
    [Serializable]
    public struct SThresholdLevel
    {
        public float speed;
        public float damage;
    }
    
    public EProjectileType type;
    [NonSerialized]
    public int playerIndex;
    public float startingDamage;

    [NonSerialized]
    public float currentDamage;

    [NonSerialized]
    public float speed = 1f;
    
    [NonSerialized]
    public int thresholdIndex = -1;

    public List<SThresholdLevel> thresholdLevels;

    [NonSerialized] public bool isLaunched;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile proj = other.transform.GetComponent<Projectile>();

        if (proj && proj.isLaunched)
        {
            if (proj.type > type)
            {
                if (thresholdLevels.Count - 1 != thresholdIndex || type + 1 != proj.type)
                    Die();
            }
            else if(proj.type < type)
            {
                if (proj.thresholdLevels.Count - 1 == proj.thresholdIndex && proj.type + 1 == type)
                    proj.Die();
            }
            else if(proj.thresholdIndex > thresholdIndex)
            {
                Die();
            }
            else if(proj.thresholdIndex < thresholdIndex)
            {
                proj.Die();
            }
        }
        else
        {
            PlayerHealth player = other.transform.GetComponent<PlayerHealth>();

            //TODO: check if it's the creator
            if (player && player.GetComponent<PlayerController>().playerIndex != playerIndex)
            {
                player.TakeDamage(currentDamage);
                Die();
            }
        }
    }

    public void SetThresholdLevel(int level)
    {
        thresholdIndex = level;

        speed = thresholdLevels[thresholdIndex].speed;
        currentDamage = startingDamage * thresholdLevels[thresholdIndex].damage;
    }

    public void Die()
    {
        //TODO: add kaboom
        Destroy(gameObject);
    }
}
