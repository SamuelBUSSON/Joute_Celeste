using System;
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

    
    public EProjectileType type;
    [NonSerialized]
    public int playerIndex;
    public float startingDamage;

    [NonSerialized]
    public float currentDamage;

    [NonSerialized]
    public float speed = 1f;


    public float speedLv1;
    public float damageLv1;
    
    public float speedLv2;
    public float damageLv2;
    

    [NonSerialized] public bool isLaunched;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile proj = other.transform.GetComponent<Projectile>();

        if (proj && proj.isLaunched)
        {
            if(proj.type > type)
               Die();
            else if(proj.type < type)
            {
                proj.Die();
            }
            else
            {
                proj.Die();
                Die();
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

    public void Die()
    {
        //TODO: add kaboom
        Destroy(gameObject);
    }
}
