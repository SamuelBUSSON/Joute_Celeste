using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public GameObject ExplosionFX;

    public float size = 1.0f;

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

    private void Awake()
    {
        currentDamage = startingDamage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile proj = other.transform.GetComponent<Projectile>();

        if (proj && proj.isLaunched)
        {
            if (proj.type > type)
            {
                if (thresholdLevels.Count - 1 != thresholdIndex || type + 1 != proj.type || proj.type == EProjectileType.PLANET)
                    Die(other.contacts[0].point);
            }
            else if(proj.type < type)
            {
                if (proj.thresholdLevels.Count - 1 == proj.thresholdIndex && proj.type + 1 == type || type == EProjectileType.PLANET)
                    proj.Die(other.contacts[0].point);
            }
            else if(proj.thresholdIndex > thresholdIndex)
            {
                Die(other.contacts[0].point);
            }
            else if(proj.thresholdIndex < thresholdIndex)
            {
                proj.Die(other.contacts[0].point);
            }
        }
        else if(isLaunched)
        {
            PlayerHealth player = other.transform.GetComponent<PlayerHealth>();
            if (player) 
            {
                if (type == EProjectileType.STAR || player.GetComponent<PlayerController>().playerIndex != playerIndex)
                {
                    player.TakeDamage(currentDamage);
                    Die(other.contacts[0].point);
                }
            } 
            else if(proj.type == EProjectileType.STAR)
                Die(other.contacts[0].point);
        }
        else if(proj && proj.type == EProjectileType.STAR)
            Die(other.contacts[0].point);
    }

    public void SetThresholdLevel(int level)
    {
        thresholdIndex = level;

        speed = thresholdLevels[thresholdIndex].speed;
        currentDamage = startingDamage * thresholdLevels[thresholdIndex].damage;
    }

    public void Die(Vector2 contactPoint)
    {
        //TODO: add kaboom
        Instantiate(ExplosionFX, contactPoint, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
