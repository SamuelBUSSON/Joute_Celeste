using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerZoneAim : MonoBehaviour
{
    

    private List<Transform> objectInZone;
    
    private Transform player;

    void Awake()
    {
        objectInZone = new List<Transform>();
    }

    private void Start()
    {
        player = GetComponentInParent<Displacement>().transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.transform)
        {
            objectInZone.Add(other.transform);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectInZone.Remove(collision.transform);
    }

    public Transform GetNearestObjectInZone()
    {
        SortList();
        return objectInZone.Count > 0 ? objectInZone[0] : null;
    }

    private void SortList()
    {
        int i = 0;
        foreach (var item in objectInZone)
        {
            if (!item)
            {
                objectInZone.RemoveAt(i);
            }
            i++;
        }


        if(objectInZone.Count > 1)
            objectInZone.Sort((t1, t2) => Vector3.Distance(t1.position, player.position).CompareTo(Vector3.Distance(t2.position, player.position)));
    }
}
