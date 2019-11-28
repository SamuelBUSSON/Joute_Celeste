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
    public int damage;

    [NonSerialized] public bool isLaunched;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile proj = other.transform.GetComponent<Projectile>();

        if (proj && proj.isLaunched)
        {
            if(proj.type > type)
                Destroy(gameObject); //TODO: add the kaboom
            else if(proj.type < type)
            {
                //TODO: add the kaboom
                Destroy(proj.gameObject);
            }
            else
            {
                //TODO: add the kaboom
                Destroy(proj.gameObject);
                Destroy(gameObject);
            }
        }
        else
        {
            PlayerHealth player = other.transform.GetComponent<PlayerHealth>();

            //TODO: check if it's the creator
            if (player && player.GetComponent<PlayerController>().playerIndex != playerIndex)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
